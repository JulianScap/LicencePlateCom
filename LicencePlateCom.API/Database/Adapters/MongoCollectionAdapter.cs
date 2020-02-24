using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LicencePlateCom.API.Database.Entities;
using MongoDB.Driver;

namespace LicencePlateCom.API.Database.Adapters
{
    public class MongoCollectionAdapter<T>
        where T : ICollectible, new()
    {
        private readonly IMongoCollection<T> _collection;

        public MongoCollectionAdapter(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Used in tests")]
        [Obsolete("Do not use", true)]
        public MongoCollectionAdapter() { }

        public virtual async Task InsertOneAsync(T item)
        {
            await _collection.InsertOneAsync(item).ConfigureAwait(false);
        }

        public virtual async Task<IAsyncCursor<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _collection.FindAsync(expression).ConfigureAwait(false);
        }
    }
}