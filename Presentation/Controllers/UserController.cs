using Application.Users.Commands.CreateUser;
using Application.Users.Commands.DeleteMyAccount;
using Application.Users.Commands.DeleteRole;
using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.SwitchRole;
using Application.Users.Commands.UpdatePassword;
using Application.Users.Commands.UpdateUserName;
using Application.Users.Commands.UpdateUserProfile;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QueueHub.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id), cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllUsersQuery(),cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("Profile")]
    public async Task<IActionResult> UpdateProfile(UpdateUserProfileCommand profileCommand
        , CancellationToken cancellationToken)
    {
        var result=await mediator.Send(profileCommand, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("UserName")]
    public async Task<IActionResult> UpdateUserName(UpdateUserNameCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    [HttpPut("Password")]
    public async Task<IActionResult> UpdateUserName(UpdatePasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new SoftDeleteUserCommand(id), cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    
    [HttpDelete("DeleteAccount")]
    public async Task<IActionResult> DeleteAccount(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteMyAccountCommand(), cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    
    [HttpDelete("DeleteRole")]
    public async Task<IActionResult> DeleteRole(int id,CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteRoleCommand(id), cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    
    [HttpPost("SwitchRole")]
    public async Task<IActionResult> SwitchRole(int id,CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new SwitchRoleCommand(id), cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}