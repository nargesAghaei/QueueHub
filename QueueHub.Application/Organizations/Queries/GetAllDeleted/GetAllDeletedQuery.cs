using Application.Common.Pagination;
using Application.Organizations.DTOs;
using MediatR;
using Shared;

namespace Application.Organizations.Queries.GetAllDeleted;

public record GetAllDeletedQuery(
    PageRequest Page,
    Guid? CategoryId):IRequest<Result<PageResult<OrganizationDto>>>;