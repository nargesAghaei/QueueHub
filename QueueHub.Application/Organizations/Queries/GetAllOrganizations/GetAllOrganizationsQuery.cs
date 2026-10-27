using Application.Common.Pagination;
using Application.Organizations.DTOs;
using Domain.Entities;
using MediatR;
using Shared;

namespace Application.Organizations.Queries.GetAllOrganizations;

public record GetAllOrganizationsQuery(
    Guid? CategoryId,
    Guid? ServiceId,
    PageRequest Page
) : IRequest<Result<PageResult<OrganizationDto>>>;