using Application.Organizations.DTOs;
using Domain.Entities;
using MediatR;
using Shared;

namespace Application.Organizations.Queries.GetAllOrganizations;

public record GetAllOrganizationsQuery(
    Guid CategoryId,
    Guid? ServiceId
) : IRequest<Result<List<OrganizationDto>>>
{
    public Guid CategoryId { get; set; } = CategoryId;
    public Guid? ServiceId { get; set; } = ServiceId;
}