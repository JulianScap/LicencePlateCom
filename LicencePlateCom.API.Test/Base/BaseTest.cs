﻿using LicencePlateCom.API.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LicencePlateCom.API.Test.Base
{
    public abstract class BaseTest
    {
        protected IConfiguration Configuration { get; }
        private readonly ServiceCollection _serviceCollection;
        private readonly ServiceProvider _serviceProvider;
        private readonly ILoggerFactory _loggerFactory;

        protected BaseTest()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _serviceCollection = new ServiceCollection();

            _serviceProvider = _serviceCollection
                .AddLogging()
                .BuildServiceProvider();

            _loggerFactory = _serviceProvider.GetService<ILoggerFactory>();
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

        protected ILogger<T> GetLogger<T>()
        {
            return _loggerFactory.CreateLogger<T>();
        }
    }
}