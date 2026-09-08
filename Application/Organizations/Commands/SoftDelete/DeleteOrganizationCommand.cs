using MediatR;
using Shared;

namespace Application.Organizations.Commands.SoftDelete;

public record DeleteOrganizationCommand(Guid Id) : IRequest<Result>
{
    public Guid OrganizationId { get; set; } = Id;
}