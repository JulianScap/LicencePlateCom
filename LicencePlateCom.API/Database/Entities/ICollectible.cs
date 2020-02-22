using Newtonsoft.Json;

namespace LicencePlateCom.API.Database.Entities
{
    public interface ICollectible
    {
        [JsonIgnore]
        public string Collection { get; }
    }
}