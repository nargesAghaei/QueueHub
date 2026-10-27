using Application.Common.Pagination;
using Application.Organizations.Commands.ActivateOrganization;
using Application.Organizations.Commands.CreateOrganization;
using Application.Organizations.Commands.DeactivateOrganization;
using Application.Organizations.Commands.Delete;
using Application.Organizations.Commands.SoftDelete;
using Application.Organizations.Commands.UpdateOrganization;
using Application.Organizations.Queries.GetAllDeleted;
using Application.Organizations.Queries.GetAllOrganizations;
using Application.Organizations.Queries.GetDeleted;
using Application.Organizations.Queries.GetOrganizationById;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QueueHub.Controllers;

[ApiController]
[Route("api/organizations")]
public class OrganizationController(IMediator mediator) : ApiControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOrganization(
        [FromBody] CreateOrganizationCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return ToCreatedResult(result);
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet]
    public async Task<IActionResult> GetAllDeleted(
        [FromQuery] GetAllDeletedQuery query,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllDeletedQuery(
            query.Page,
            query.CategoryId));

        return ToActionResult(result);
    }

    
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllOrganizationsQuery query,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllOrganizationsQuery(
            query.CategoryId,
            query.ServiceId,
            query.Page));

        return ToActionResult(result);
    }
    
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetOrganizationByIdQuery(id), ct);

        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet("/{id:guid}")]
    public async Task<IActionResult> GetDeletedById(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new GetDeletedOrganQuery(id), ct);

        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Admin)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateOrganizationCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new UpdateOrganizationCommand(), ct);

        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new DeleteOrganizationCommand(id), ct);

        return ToActionResult(result);
    }
    
    
    [Authorize(Roles = RoleNames.Admin)]
    [HttpDelete("hardDelete/{id:guid}")]
    public async Task<IActionResult> HardDelete(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new DeleteOrganCommand(id), ct);

        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Admin)]
    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new ActivateOrganizationCommand(id), ct);

        return ToActionResult(result);
    }

    
    [Authorize(Roles = RoleNames.Admin)]
    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new DeactivateOrganizationCommand(id), ct);

        return ToActionResult(result);
    }
}