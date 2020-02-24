using System.Threading.Tasks;
using LicencePlateCom.API.Database;
using LicencePlateCom.API.Test.Base;
using LicencePlateCom.API.Test.Entities;
using Xunit;

namespace LicencePlateCom.API.Test.Database
{
    public class MongoContextShould : BaseTest
    {
        private readonly DbAccessTestClass _dummy = new DbAccessTestClass {Name = "Test"};

        private IMongoContext<DbAccessTestClass> GetContext(bool success = true)
        {
            return base.GetContext(() => new[] {_dummy}, success);
        }

        [Fact]
        public async Task ReturnTrueWhenAddSucceeds()
        {
            var result = await GetContext(false).AddAsync(_dummy);
            Assert.False(result);
        }

        [Fact]
        public async Task ReturnFalseWhenSaveFails()
        {
            var result = await GetContext().AddAsync(_dummy);
            Assert.True(result);
        }

        [Fact]
        public async Task GetSuccessfully()
        {
            var result = await GetContext()
                .GetAsync(ex => ex.Name == "Test");

            Assert.NotEmpty(result);
        }
    }
}