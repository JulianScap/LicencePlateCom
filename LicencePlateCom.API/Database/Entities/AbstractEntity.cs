using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace LicencePlateCom.API.Database.Entities
{
    public abstract class AbstractEntity : ICollectible, IEntity
    {
        private readonly Lazy<string> _collection;

        protected AbstractEntity()
        {
            _collection = new Lazy<string>(GetCollectionName);
        }

        private string GetCollectionName() => GetType().Name.ToLowerInvariant();

        [JsonIgnore] public string Collection => _collection.Value;

        public override string ToString()
        {
            return $"{GetType().Name} - {JsonConvert.SerializeObject(this, Formatting.None)}";
        }

        [JsonIgnore]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}