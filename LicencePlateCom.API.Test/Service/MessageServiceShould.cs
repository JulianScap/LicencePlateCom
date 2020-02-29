using System.Threading.Tasks;
using LicencePlateCom.API.Business;
using LicencePlateCom.API.Database.Entities;
using LicencePlateCom.API.Test.Base;
using Xunit;

namespace LicencePlateCom.API.Test.Service
{
    public class MessageServiceShould : BaseTest
    {
        private readonly Message _message = new Message
        {
            Recipient = "VDF132",
            PredefinedMessage = PredefinedMessage.WeirdNoise
        };

        protected virtual Message[] GetMessage() => new[] {_message};

        protected virtual IMessageService GetMessageService(bool success = true, bool throws = false)
        {
            return new MessageService(GetContext(GetMessage, success, throws), GetLogger<MessageService>());
        }

        [Fact]
        public async Task ValidateOnSave()
        {
            var service = GetMessageService();
            var result = await service.SaveMessageAsync(new Message
            {
                PredefinedMessage = PredefinedMessage.None
            });

            Assert.False(result.Success);
            Assert.NotEmpty(result.Messages);
        }

        [Fact]
        public async Task ValidateOnSaveNull()
        {
            var service = GetMessageService();
            var result = await service.SaveMessageAsync(null);

            Assert.False(result.Success);
            Assert.NotEmpty(result.Messages);
        }

        [Fact]
        public async Task ReturnTrueOnSuccessfulSave()
        {
            var service = GetMessageService();
            var result = await service.SaveMessageAsync(_message);

            Assert.True(result.Success);
        }

        [Fact]
        public async Task ReturnFalseOnFailedSave()
        {
            var service = GetMessageService(false);
            var result = await service.SaveMessageAsync(_message);

            Assert.False(result.Success);
        }

        [Fact]
        public async Task ReturnFalseOnException()
        {
            var service = GetMessageService(false, true);
            var resultSave = await service.SaveMessageAsync(_message);
            var resultGet = await service.GetMessagesAsync(x => x.Id == "test");

            Assert.False(resultSave.Success);
            Assert.False(resultGet.Success);
        }

        [Fact]
        public async Task GetMessages()
        {
            var service = GetMessageService();
            var result = await service.GetMessagesAsync(m => m.Id == null);

            Assert.True(result.Success);
        }
    }
}