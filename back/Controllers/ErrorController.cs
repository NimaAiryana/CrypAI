using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using back.Models;

namespace back.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    [Route("/api/error")]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        
        if (exception != null)
        {
            _logger.LogError(exception, "Unhandled exception occurred");
        }
        
        // Don't expose the actual error details in production
        return StatusCode(500, new ErrorResponse
        {
            Message = "An unexpected error occurred. Please try again later."
        });
    }
}
