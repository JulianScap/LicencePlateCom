using System.Collections.Generic;
using LicencePlateCom.API.Database.Entities;
using Xunit;

namespace LicencePlateCom.API.Test.Database
{
    public class MessageShould
    {
        public readonly Dictionary<string, Message> Messages = new Dictionary<string, Message>
        {
            {Message.NoMessage, new Message{ Id = "123", PredefinedMessage = PredefinedMessage.None, Recipient = "123"}},
            {Message.NoRecipient, new Message{ Id = "123", PredefinedMessage = PredefinedMessage.FlatTire, Recipient = "     "}},
            {Message.RecipientTooLong, new Message{ Id = "123", PredefinedMessage = PredefinedMessage.None, Recipient = "12345678901"}},
        };

        [Fact]
        public void ValidateProperly()
        {
            foreach (var (validationMessage, messageEntity) in Messages)
            {
                Assert.False(messageEntity.Validate(out var validationMessages));
                Assert.NotEmpty(validationMessages);
                Assert.Contains(validationMessage, validationMessages);
            }
        }

        [Fact]
        public void ValidateMultiple()
        {
            var message = new Message
            {
                PredefinedMessage = PredefinedMessage.None,
                Recipient = null
            };

            Assert.False(message.Validate(out var validationMessages));
            Assert.NotEmpty(validationMessages);
            Assert.Contains(Message.NoMessage, validationMessages);
            Assert.Contains(Message.NoRecipient, validationMessages);
        }
    }
}