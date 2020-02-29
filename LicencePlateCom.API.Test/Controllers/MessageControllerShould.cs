using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LicencePlateCom.API.Business;
using LicencePlateCom.API.Controllers;
using LicencePlateCom.API.Database.Entities;
using LicencePlateCom.API.Test.Base;
using LicencePlateCom.API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LicencePlateCom.API.Test.Controllers
{
    public class MessageControllerShould : BaseTest
    {
        public MessageController GetController(bool success = true)
        {
            var messageService = new Mock<MessageService>();

            var list = new List<Message>
            {
                new Message
                {
                    Id = "ABC",
                    Recipient = "XXX456",
                    PredefinedMessage = PredefinedMessage.WeirdNoise
                }
            };

            messageService
                .Setup(x => x.SaveMessageAsync(It.IsAny<Message>()))
                .ReturnsAsync((Result)success);
            messageService
                .Setup(x => x.GetMessagesAsync(It.IsAny<Expression<Func<Message, bool>>>()))
                .ReturnsAsync(success ? Return.Success(list) : Return.Failed<List<Message>>());

            return new MessageController(messageService.Object, GetLogger<MessageController>());
        }

        [Fact]
        public async Task SuccessOnGetMessages()
        {
            var controller = GetController();
            var result = await controller.Get("ABC123") as OkObjectResult;
            Assert.NotNull(result?.Value);
        }

        [Fact]
        public async Task ErrorOnGetEmptyPlate()
        {
            var controller = GetController();
            var result = await controller.Get(null) as BadRequestObjectResult;
            var errorMessage = (result?.Value as Error)?.Text;
            Assert.NotNull(result);
            Assert.NotNull(errorMessage);
        }

        [Fact]
        public async Task ErrorOnPutNullMessage()
        {
            var controller = GetController();
            var result = await controller.Put(null) as BadRequestResult;
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ReturnOkOnSuccess()
        {
            var controller = GetController();
            var result = await controller.Put(new Message()
            {
                Id = "FSD",
                Recipient = "TRE456",
                PredefinedMessage = PredefinedMessage.BrokenTailLight
            }) as OkResult;
            Assert.NotNull(result);
        }
    }
}