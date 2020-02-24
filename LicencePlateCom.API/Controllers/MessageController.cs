using System.Threading.Tasks;
using LicencePlateCom.API.Business;
using LicencePlateCom.API.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LicencePlateCom.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : BaseController
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IMessageService _messageService;

        public MessageController(ILogger<MessageController> logger, IMessageService messageService) : base(logger)
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
        public async Task<IActionResult> Put([FromBody] Message message)
        {
            if (!Validate(message, out var messages, nameof(message)))
            {
                return BadRequest(messages);
            }

            var saveResult = await _messageService.SaveMessageAsync(message);

            return Result(saveResult);
        }
    }
}