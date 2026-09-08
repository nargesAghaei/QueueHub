using MediatR;
using Shared;

namespace Application.Organizations.Commands.DeactivateOrganization;

public record DeactivateOrganizationCommand(Guid Id) : IRequest<Result>
{
    public Guid OrganizationId { get; set; } = Id;
}