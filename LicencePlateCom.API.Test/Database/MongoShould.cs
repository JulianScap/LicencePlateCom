using System.Linq;
using System.Threading.Tasks;
using LicencePlateCom.API.Test.Base;
using LicencePlateCom.API.Test.Entities;
using MongoDB.Driver;
using Xunit;

namespace LicencePlateCom.API.Test.Database
{
    public class MongoShould : BaseTest
    {
        class CountResult
        {
            public string Name { get; set; }
            public int Total { get; set; }
        }
        
        [Fact]
        public async Task LetMeGroup()
        {
            const string name = "Default Name";
            const int count = 5;

            var settings = GetSettings();
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            var collection = database.GetCollection<DbAccessTestClass>("dbaccesstestclass");

            await collection.DeleteManyAsync(x => true).ConfigureAwait(false);

            foreach (var _ in Enumerable.Range(0, count))
            {
                await collection.InsertOneAsync(new DbAccessTestClass{Name = name});
            }
            
            var result = await collection
                .Aggregate()
                .Match(x => x.Name == name)
                .Group(x => x.Name, g => new CountResult { Name = g.Key, Total = g.Sum(x => 1)})
                .ToListAsync()
                .ConfigureAwait(false);

            Assert.Equal(count, result.First(x => x.Name == name).Total);
        }
    }
}
