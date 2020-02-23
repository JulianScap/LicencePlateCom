using System;
using System.Linq.Expressions;
using LicencePlateCom.API.Database;
using LicencePlateCom.API.Database.Adapters;
using LicencePlateCom.API.Test.Base;
using LicencePlateCom.API.Test.Entities;
using LicencePlateCom.API.Test.Fake;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace LicencePlateCom.API.Test.Database
{
    public class MongoContextShould : BaseTest
    {
        private IMongoContext<DbAccessTestClass> GetContext(bool success = true)
        {
            var logger = GetLogger<MongoContext<DbAccessTestClass>>();
            var collection = new Mock<MongoCollectionAdapter<DbAccessTestClass>>();


            var insertOneSetup = collection
                .Setup(x =>
                    x.InsertOne(It.IsAny<DbAccessTestClass>()));
            if (!success)
            {
                insertOneSetup.Throws<Exception>();
            }

            var testInstance = new DbAccessTestClass {Name = "Test"};
            var fakeAsyncCursor = (IAsyncCursor<DbAccessTestClass>) FakeAsyncCursor.CreateInstance(testInstance);
            collection
                .Setup(x => x.FindAsync(It.IsAny<Expression<Func<DbAccessTestClass, bool>>>()))
                .ReturnsAsync(fakeAsyncCursor);

            return new MongoContext<DbAccessTestClass>(logger, collection.Object);
        }

        [Fact]
        public void ReturnTrueWhenAddSucceeds()
        {
            var result = GetContext(false).Add(new DbAccessTestClass {Name = "Test"});
            Assert.False(result);
        }

        [Fact]
        public void ReturnFalseWhenSaveFails()
        {
            var result = GetContext().Add(new DbAccessTestClass {Name = "Test"});
            Assert.True(result);
        }

        [Fact]
        public async void GetSuccessfully()
        {
            var result = await GetContext()
                .Get(ex => ex.Name == "Test");

            Assert.NotEmpty(result);
        }
    }
}