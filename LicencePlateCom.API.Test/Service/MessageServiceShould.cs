using System.Threading.Tasks;
using LicencePlateCom.API.Business;
using LicencePlateCom.API.Database.Entities;
using LicencePlateCom.API.Test.Base;
using Xunit;

namespace LicencePlateCom.API.Test.Service
{
    public class MessageServiceShould : BaseTest
    {
        private Message _message = new Message
        {
            Recipient = "VDF132",
            PredefinedMessage = PredefinedMessage.WeirdNoise
        };

        protected virtual Message[] GetMessage() => new[] {_message};

        protected virtual IMessageService GetMessageService(bool success = true)
        {
            return new MessageService(GetContext(GetMessage, success), GetLogger<MessageService>());
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
        public async Task GetMessages()
        {
            var service = GetMessageService();
            var result = await service.GetMessagesAsync(m => m.Id == null);

            Assert.True(result.Success);
        }
    }
}