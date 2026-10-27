using Application.Branchs.DTOs;
using Application.Common.Pagination;
using MediatR;
using Shared;

namespace Application.Branchs.Queries.GetAll;

public record GetAllBranchQuery(
    PageRequest page,
    Guid? OrganId,
    Guid? ServiceId,
    Guid? CityId,
    bool? IsOpen,
    string? Search,
    bool? IsActive)
    : IRequest<Result<PageResult<BranchListDto>>>;
