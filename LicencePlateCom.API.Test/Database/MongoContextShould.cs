using System.Threading.Tasks;
using LicencePlateCom.API.Database;
using LicencePlateCom.API.Test.Base;
using LicencePlateCom.API.Test.Entities;
using Xunit;

namespace LicencePlateCom.API.Test.Database
{
    public class MongoContextShould : BaseTest
    {
        private IMongoContext GetContext()
        {
            var logger = GetLogger<MongoContext>();
            var settings = GetSettings();
            return new MongoContext(logger, settings);
        }

        [Fact]
        public void SaveSuccessfully()
        {
            var result = GetContext().Add(new DbAccessTestClass {Name = "Test"});
            Assert.True(result);
        }

        [Fact]
        public async Task GetSuccessfully()
        {
            var result = await GetContext()
                .Get<DbAccessTestClass>(ex => ex.Name == "Test")
                .ConfigureAwait(false);
            
            Assert.NotEmpty(result);
        }
    }
}