using System.Collections.Generic;
using LicencePlateCom.API.Database;
using LicencePlateCom.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LicencePlateCom.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {

        private readonly ILogger<MessageController> _logger;

        public MessageController(ILogger<MessageController> logger, IMongoContext dataAccess)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Message> Get(string licencePlate)
        {
            return null;
        }
    }
}
