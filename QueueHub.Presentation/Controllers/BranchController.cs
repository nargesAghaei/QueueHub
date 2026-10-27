using Application.Branchs.Commands.Active;
using Application.Branchs.Commands.Create;
using Application.Branchs.Commands.Delete;
using Application.Branchs.Commands.Restore;
using Application.Branchs.Commands.Update;
using Application.Branchs.Queries.GetAll;
using Application.Branchs.Queries.GetAllDeleted;
using Application.Branchs.Queries.GetBranch;
using Application.Branchs.Queries.GetDeleted;

using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QueueHub.Controllers;

[ApiController]
[Route("api/branches")]
[Authorize]
public class BranchController(IMediator mediator) : ApiControllerBase
{
    [HttpPost]
    [Authorize(Roles = RoleNames.Manager)]
    public async Task<IActionResult> Create(
        [FromBody] CreateBranchCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return ToActionResult(result);
    }
    

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllBranchQuery query,
        CancellationToken ct)
    {
        var result = await mediator.Send(query, ct);
        return ToActionResult(result);
    }

    
    [HttpGet("deleted")]
    [Authorize(Roles = RoleNames.Manager)]
    public async Task<IActionResult> GetAllDeleted(
        [FromQuery] GetAllDeletedBranchesQuery query,
        CancellationToken ct)
    {
        var result = await mediator.Send(query, ct);
        return ToActionResult(result);
    }

    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetBranchQuery(id), ct);

        return ToActionResult(result);
    }

    
    [HttpGet("deleted/{id:guid}")]
    [Authorize(Roles = RoleNames.Manager)]
    public async Task<IActionResult> GetDeletedById(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetDeletedQuery(id), ct);

        return ToActionResult(result);
    }

    
    [HttpPut("{id:guid}")]
    [Authorize(Roles = RoleNames.Manager)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateBranchCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command);

        return ToActionResult(result);
    }

    
    [HttpPatch("{id:guid}/activate")]
    [Authorize(Roles = RoleNames.Manager)]
    public async Task<IActionResult> Activate(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new ActiveBranchCommand(id), ct);

        return ToActionResult(result);
    }

    
    [HttpPatch("{id:guid}/soft-delete")]
    [Authorize(Roles = RoleNames.Manager)]
    public async Task<IActionResult> SoftDelete(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new SoftDeleteBranchCommand(id), ct);

        return ToActionResult(result);
    }

    
    [HttpPost("{id:guid}/restore")]
    [Authorize(Roles = RoleNames.Manager)]
    public async Task<IActionResult> Restore(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new RestoreBranchCommand(id), ct);

        return ToActionResult(result);
    }

    
    [HttpDelete("{id:guid}/hard-delete")]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> HardDelete(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new DeleteBranchCommand(id), ct);

        return ToActionResult(result);
    }
}
