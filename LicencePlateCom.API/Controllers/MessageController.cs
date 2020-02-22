using LicencePlateCom.API.Business;
using LicencePlateCom.API.Database.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LicencePlateCom.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IMessageService _messageService;

        public MessageController(ILogger<MessageController> logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        [HttpGet("{licencePlate}")]
        public ActionResult Get(string licencePlate)
        {
            if (string.IsNullOrWhiteSpace(licencePlate))
            {
                _logger.LogTrace("Empty licencePlate");
                return BadRequest("No license plate specified.");
            }

            return Ok(new[]
            {
                new Message
                {
                    PredefinedMessage = PredefinedMessage.WeirdNoise,
                    Recipient = licencePlate
                }
            });
        }

        [HttpPut]
        public ActionResult Put([FromBody] Message message)
        {
            if (message == null)
            {
                return BadRequest(new[] {"No message specified (null)."});
            }

            if (message.Validate(out var messages))
            {
                return BadRequest(messages);
            }

            var saveResult = _messageService.SaveMessage(message);

            return saveResult ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}