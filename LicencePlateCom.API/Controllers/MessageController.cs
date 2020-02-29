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

        public MessageController(IMessageService messageService, ILogger<MessageController> logger) : base(logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        [HttpGet("{licencePlate}")]
        public async Task<IActionResult> Get(string licencePlate)
        {
            if (string.IsNullOrWhiteSpace(licencePlate))
            {
                _logger.LogTrace("Empty licencePlate");
                return BadRequest("No license plate specified.");
            }

            var result = await _messageService.GetMessagesAsync(x => x.Recipient == licencePlate).ConfigureAwait(false);

            return Ok(result.Item);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Message message)
        {
            if (message == null)
            {
                return BadRequest();
            }

            var saveResult = await _messageService.SaveMessageAsync(message);
            return Result(saveResult);
        }
    }
}