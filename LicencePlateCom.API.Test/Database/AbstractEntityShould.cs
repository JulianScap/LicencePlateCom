using LicencePlateCom.API.Test.Entities;
using Xunit;

namespace LicencePlateCom.API.Test.Database
{
    /// <summary>
    /// #overkill
    /// </summary>
    public class AbstractEntityShould
    {
        [Fact]
        public void GetClassNameSuccessfully()
        {
            var collectionName = new DbAccessTestClass().Collection;
            // ReSharper disable once StringLiteralTypo
            Assert.Equal("dbaccesstestclass", collectionName);
        }
        
        [Fact]
        public void AssignIdAndNotChangeIt()
        {
            const string myId = "My Id";
            var dbAccessTestClass = new DbAccessTestClass
            {
                Id = myId
            };
            
            Assert.Equal(myId, dbAccessTestClass.Id);
        }
    }
}