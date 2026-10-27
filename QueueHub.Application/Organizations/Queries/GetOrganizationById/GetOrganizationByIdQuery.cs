using Application.Organizations.DTOs;
using MediatR;
using Shared;

namespace Application.Organizations.Queries.GetOrganizationById;

public record GetOrganizationByIdQuery(Guid Id)
    : IRequest<Result<OrganizationDto>>
{
    public Guid OrganizationId { get; set; } = Id;
}