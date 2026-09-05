using Application.Users.Commands.CreateEmployeeCommand;
using Application.Users.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QueueHub.Application.Users.Commands.Login;

namespace QueueHub.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator):ControllerBase
{
    [HttpPost("RegisterCitizen")]
    public async Task<IActionResult> RegisterCitizen([FromBody] CreateCitizenCommand Command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(Command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("RegisterStaff")]
    public async Task<IActionResult> RegisterStaff([FromBody] CreateStaffCommand Command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(Command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }
}