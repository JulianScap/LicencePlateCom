using System.Linq;
using LicencePlateCom.API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LicencePlateCom.API.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly ILogger _logger;

        public BaseController(ILogger logger)
        {
            _logger = logger;
        }
        
        public BadRequestObjectResult BadRequest(string error)
        {
            return base.BadRequest(new Error{Text = error});
        }

        protected IActionResult Result(Result result)
        {
            if (!result.Success)
            {
                if (result.Messages == null || !result.Messages.Any())
                {
                    _logger.LogWarning("Failed request without context");
                    return BadRequest();
                }

                return BadRequest(result.Messages);
            }

            return Ok();
        }

        protected IActionResult Result<T>(Result<T> result)
            where T : class
        {
            if (!result.Success)
            {
                if (result.Messages == null || !result.Messages.Any())
                {
                    _logger.LogWarning("Failed request without context");
                    return BadRequest();
                }

                return BadRequest(result.Messages);
            }

            return result.HasItem ? (IActionResult) Ok(result.Item) : Ok();
        }
    }

    public class Error
    {
        public string Text { get; set; }
    }
}