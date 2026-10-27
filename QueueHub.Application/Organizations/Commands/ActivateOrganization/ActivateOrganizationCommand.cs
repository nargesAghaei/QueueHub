using MediatR;
using Shared;

namespace Application.Organizations.Commands.ActivateOrganization;

public record ActivateOrganizationCommand(Guid Id) : IRequest<Result>
{
    public Guid OrganizationId { get; set; } = Id;
}