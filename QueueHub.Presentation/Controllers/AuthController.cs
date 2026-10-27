using Application.Auth.Commands.CreateCitizen;
using Application.Auth.Commands.CreateStaff;
using Application.Auth.Commands.Login;
using Application.Organizations.Commands.CreateManager;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QueueHub.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator mediator) : ApiControllerBase
{
    [AllowAnonymous]
    [HttpPost("register/citizen")]
    public async Task<IActionResult> RegisterCitizen(
        [FromBody] CreateCitizenCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return ToCreatedResult(result);
    }

    [Authorize(Roles = RoleNames.Manager)]
    [HttpPost("register/staff")]
    public async Task<IActionResult> RegisterStaff(
        [FromBody] CreateStaffCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return ToCreatedResult(result);
    }

    [AllowAnonymous]
    [HttpPost("register/manager")]
    public async Task<IActionResult> RegisterManager(
        [FromBody] CreateManagerCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return ToCreatedResult(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return ToActionResult(result);
    }
}