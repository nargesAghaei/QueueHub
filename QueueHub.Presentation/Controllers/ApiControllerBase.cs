using Microsoft.AspNetCore.Mvc;
using Shared;

namespace QueueHub.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult ToActionResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(result.Data);

        return ToProblem(result);
    }

    protected IActionResult ToActionResult(Result result)
    {
        if (result.IsSuccess)
            return NoContent();

        return ToProblem(result);
    }

    protected IActionResult ToProblem(Result result)
    {
        var error = result.Error!;

        return Problem(
            statusCode: ToStatusCode(error.Type),
            detail: error.Message);
    }
    
    protected IActionResult ToCreatedResult<T>(Result<T> result)
    {
        if (result.IsFailure)
            return ToProblem(result);

        return StatusCode(
            StatusCodes.Status201Created,
            result.Data);
    }

    private static int ToStatusCode(ErrorType type) => type switch
    {
        ErrorType.Validation   => StatusCodes.Status400BadRequest,
        ErrorType.NotFound     => StatusCodes.Status404NotFound,
        ErrorType.Conflict     => StatusCodes.Status409Conflict,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden    => StatusCodes.Status403Forbidden,
        _                      => StatusCodes.Status400BadRequest
    };
}