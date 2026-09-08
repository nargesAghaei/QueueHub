using Application.Organizations.Commands.ActivateOrganization;
using Application.Organizations.Commands.CreateOrganization;
using Application.Organizations.Commands.DeactivateOrganization;
using Application.Organizations.Commands.SoftDelete;
using Application.Organizations.Commands.UpdateOrganization;
using Application.Organizations.Queries.GetAllOrganizations;
using Application.Organizations.Queries.GetOrganizationById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QueueHub.Controllers;

[ApiController]
[Route("api/organizations")]
public class OrganizationController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOrganization([FromBody] CreateOrganizationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid categoryId,[FromQuery] Guid? ServiceId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllOrganizationsQuery(categoryId,ServiceId),cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute]Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOrganizationByIdQuery(id),cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    
    [Authorize(Roles = "Manager")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromBody] UpdateOrganizationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateOrganizationCommand(),cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }


    [Authorize(Roles = "Admin, Manager")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteOrganizationCommand(id),cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ActivateOrganizationCommand(id),cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeactivateOrganizationCommand(id),cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}