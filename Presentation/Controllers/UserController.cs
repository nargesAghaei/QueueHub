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
using QueueHub.Application.Users.Queries.GetStaff;

namespace QueueHub.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id), cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool isDeleted,[FromQuery] int? roleId,CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllUsersQuery(isDeleted,roleId),cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    
    [Authorize(Roles = "Manager")]
    [HttpGet]
    public async Task<IActionResult> GetStaffs([FromQuery] Guid? branchId,CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetStaffQuery(branchId),cancellationToken);
            if (!result.IsSuccess)
                return BadRequest(result);
    
            return Ok(result);
        }
    
    
    [Authorize(Roles = "Citizen,Admin")]
    [HttpPut("Profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileCommand profileCommand
        , CancellationToken cancellationToken)
    {
        var result=await mediator.Send(profileCommand, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    
    [HttpPut("UserName")]
    public async Task<IActionResult> UpdateUserName([FromBody] UpdateUserNameCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    
    
    [HttpPut("Password")]
    public async Task<IActionResult> UpdateUserName([FromBody] UpdatePasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    
    [Authorize(Roles = "Admin,Manager")]
    [HttpDelete("DeleteUser/{id:guid}")]
    public async Task<IActionResult> DeleteUser([FromRoute]Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new SoftDeleteUserCommand(id), cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    
    
    [Authorize(Roles = "Citizen")]
    [HttpDelete("DeleteAccount")]
    public async Task<IActionResult> DeleteAccount(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteMyAccountCommand(), cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    
    
    [Authorize(Roles = "Citizen")]
    [HttpDelete("DeleteRole/{roleId}")]
    public async Task<IActionResult> DeleteRole([FromRoute]int roleId,CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteRoleCommand(roleId), cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    
    
    [HttpPost("SwitchRole/{roleId}")]
    public async Task<IActionResult> SwitchRole([FromRoute]int roleId,CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new SwitchRoleCommand(roleId), cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}