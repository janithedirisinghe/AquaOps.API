using AquaOps.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace AquaOps.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult FromResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(result.Data);

        return StatusCode(result.StatusCode, new { error = result.Error });
    }

    protected IActionResult FromResult(Result result)
    {
        if (result.IsSuccess)
            return NoContent();

        return StatusCode(result.StatusCode, new { error = result.Error });
    }

    protected IActionResult CreatedFromResult<T>(Result<T> result, string? routeName = null, object? routeValues = null)
    {
        if (result.IsSuccess)
        {
            if (routeName != null)
                return CreatedAtRoute(routeName, routeValues, result.Data);
            return StatusCode(201, result.Data);
        }

        return StatusCode(result.StatusCode, new { error = result.Error });
    }
}
