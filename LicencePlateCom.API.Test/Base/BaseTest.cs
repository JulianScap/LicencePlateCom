using System;
using System.Linq.Expressions;
using LicencePlateCom.API.Database;
using LicencePlateCom.API.Database.Adapters;
using LicencePlateCom.API.Database.Entities;
using LicencePlateCom.API.Test.Fake;
using LicencePlateCom.API.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;

namespace LicencePlateCom.API.Test.Base
{
    public abstract class BaseTest
    {
        private IConfiguration Configuration { get; }
        private readonly ILoggerFactory _loggerFactory;

        protected BaseTest()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            _loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        }

        protected virtual IOptions<Settings> GetSettings()
        {
            var settings = new Settings
            {
                ConnectionString = Configuration.GetConnectionString("LicencePlateCom"),
                DatabaseName = Configuration.GetSection("Database")["Name"]
            };

            return Options.Create(settings);
        }

        protected virtual ILogger<T> GetLogger<T>()
        {
            return _loggerFactory.CreateLogger<T>();
        }

        protected virtual IMongoContext<T> GetContext<T>(Func<T[]> getTestInstance, bool success = true, bool throws = false)
            where T : ICollectible, new()
        {
            if (throws)
            {
                var context = new Mock<MongoContext<T>>();
                context.Setup(x => x.AddAsync(It.IsAny<T>()))
                    .Throws<Exception>();
                context.Setup(x => x.GetAsync(It.IsAny<Expression<Func<T, bool>>>()))
                    .Throws<Exception>();
                return context.Object;
            }
            var logger = GetLogger<MongoContext<T>>();
            var collection = new Mock<MongoCollectionAdapter<T>>();

            var insertOneSetup = collection
                .Setup(x =>
                    x.InsertOneAsync(It.IsAny<T>()));
            if (!success)
            {
                insertOneSetup.Throws<Exception>();
            }

            var obj = FakeAsyncCursor.CreateInstance(getTestInstance());
            var fakeAsyncCursor = (IAsyncCursor<T>) obj;
            collection
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<T, bool>>>()))
                .ReturnsAsync(fakeAsyncCursor);

            return new MongoContext<T>(logger, collection.Object);
        }
    }
}