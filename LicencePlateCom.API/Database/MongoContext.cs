using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LicencePlateCom.API.Database.Entities;
using LicencePlateCom.API.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LicencePlateCom.API.Database
{
    public interface IMongoContext
    {
        bool Add<T>(T item) where T : ICollectible;
        Task<IEnumerable<T>> Get<T>(Expression<Func<T, bool>> expression) where T : ICollectible, new();
        Task<IEnumerable<T>> Get<T>(Expression<Func<T, bool>> expression, T example) where T : ICollectible;
    }

    public class MongoContext : IMongoContext
    {
        private readonly ILogger<MongoContext> _logger;
        private readonly IMongoDatabase _database;

        public MongoContext(ILogger<MongoContext> logger, IOptions<Settings> settings)
        {
            _logger = logger;
            IMongoClient client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public bool Add<T>(T item)
            where T : ICollectible
        {
            try
            {
                var collection = _database.GetCollection<T>(item.Collection);
                collection.InsertOne(item);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured where running MongoContext.Add {item.ToString()}");
                return false;
            }
        }

        public async Task<IEnumerable<T>> Get<T>(Expression<Func<T, bool>> expression)
            where T : ICollectible, new()
        {
            return await Get(expression, new T()).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> Get<T>(Expression<Func<T, bool>> expression, T example) where T : ICollectible
        {
            var collection = _database.GetCollection<T>(example.Collection);
            var found = await collection.FindAsync(expression).ConfigureAwait(false);

            return await found.ToListAsync().ConfigureAwait(false);
        }
    }
}