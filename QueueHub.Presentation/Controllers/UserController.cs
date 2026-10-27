using Application.Users.Commands.DeleteMyAccount;
using Application.Users.Commands.DeleteRole;
using Application.Users.Commands.DeleteStaff;
using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.RestoreStaff;
using Application.Users.Commands.RestoreUser;
using Application.Users.Commands.SwitchRole;
using Application.Users.Commands.UpdatePassword;
using Application.Users.Commands.UpdateProfile;
using Application.Users.Commands.UpdateStaff;
using Application.Users.Commands.UpdateUserInformation;
using Application.Users.Commands.UpdateUserName;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetDeletedStaff;
using Application.Users.Queries.GetStaff;
using Application.Users.Queries.GetUserById;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QueueHub.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController(IMediator mediator) : ApiControllerBase
{
    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id), ct);
        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool isDeleted,
        [FromQuery] int? roleId,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetAllUsersQuery(isDeleted, roleId), ct);

        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Admin)]
    [HttpPut("information")]
    public async Task<IActionResult> UpdateUserInfo(
        [FromBody] UpdateUserInformationCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return ToActionResult(result);
    }
    
    
    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(new SoftDeleteUserCommand(id), ct);
        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("roles/{roleId:int}")]
    public async Task<IActionResult> DeleteRole(
        [FromRoute] int roleId,
        CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteRoleCommand(roleId), ct);
        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Admin)]
    [HttpPatch("{id:guid}/restore")]
    public async Task<IActionResult> RestoreUser(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(new RestoreUserCommand(id), ct);
        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Manager + "," + RoleNames.Admin)]
    [HttpGet("branches/{branchId:guid}/staff")]
    public async Task<IActionResult> GetStaffs(
        [FromRoute] Guid branchId,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetStaffQuery(branchId), ct);
        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Manager + "," + RoleNames.Admin)]
    [HttpGet("branches/{branchId:guid}/staff/deleted")]
    public async Task<IActionResult> GetDeletedStaffs(
        [FromRoute] Guid branchId,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetDeletedStaffQuery(branchId), ct);
        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Manager)]
    [HttpPut("staff")]
    public async Task<IActionResult> UpdateStaff(
        [FromBody] UpdateStaffCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Manager)]
    [HttpDelete("staff/{id:guid}")]
    public async Task<IActionResult> DeleteStaff(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteStaffCommand(id), ct);
        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Manager)]
    [HttpPatch("staff/{id:guid}/restore")]
    public async Task<IActionResult> RestoreStaff(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(new RestoreStaffCommand(id), ct);
        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Citizen)]
    [HttpPut("me/profile")]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateProfileCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Citizen)]
    [HttpPut("me/username")]
    public async Task<IActionResult> UpdateUserName(
        [FromBody] UpdateUserNameCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Citizen)]
    [HttpPut("me/password")]
    public async Task<IActionResult> UpdatePassword(
        [FromBody] UpdatePasswordCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Citizen)]
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteAccount(CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteMyAccountCommand(), ct);
        return ToActionResult(result);
    }

    
    [HttpPost("me/switch-role/{roleId:int}")]
    public async Task<IActionResult> SwitchRole(
        [FromRoute] int roleId,
        CancellationToken ct)
    {
        var result = await mediator.Send(new SwitchRoleCommand(roleId), ct);
        return ToActionResult(result);
    }
}