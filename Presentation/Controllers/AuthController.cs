using Application.Auth;
using Application.Auth.DTOs;
using Application.Users.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace QueueHub.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator,IAuthService authService):ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] CreateCitizenCommand command, CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return Ok(id);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
    {
        var result = await authService.Login(
            dto,
            cancellationToken);

        return Ok(result);
    }
}