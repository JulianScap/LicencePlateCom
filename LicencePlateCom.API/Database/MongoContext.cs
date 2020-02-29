using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LicencePlateCom.API.Database.Adapters;
using LicencePlateCom.API.Database.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace LicencePlateCom.API.Database
{
    public interface IMongoContext<T>
        where T : ICollectible, new()
    {
        Task<bool> AddAsync(T item);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression);
    }

    public class MongoContext<T> : IMongoContext<T>
        where T : ICollectible, new()
    {
        private readonly ILogger<MongoContext<T>> _logger;
        private readonly MongoCollectionAdapter<T> _collection;

        public MongoContext() { }
        
        public MongoContext(ILogger<MongoContext<T>> logger, MongoCollectionAdapter<T> collection)
        {
            _logger = logger;
            _collection = collection;
        }

        public virtual async Task<bool> AddAsync(T item)
        {
            try
            {
                await _collection.InsertOneAsync(item).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured where running MongoContext.Add {item.ToString()}");
                return false;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> expression)
        {
            var found = await _collection.FindAsync(expression).ConfigureAwait(false);
            return await found.ToListAsync().ConfigureAwait(false);
        }
    }
}