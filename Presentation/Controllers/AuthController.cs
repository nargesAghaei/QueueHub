using Application.Auth;
using Application.Auth.DTOs;
using Application.Users.Commands.CreateEmployeeCommand;
using Application.Users.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace QueueHub.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator,IAuthService authService):ControllerBase
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
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        var result = await authService.Login(dto, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }
}