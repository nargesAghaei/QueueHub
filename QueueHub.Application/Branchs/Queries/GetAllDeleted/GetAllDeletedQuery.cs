using Application.Branchs.DTOs;
using Application.Common.Pagination;
using MediatR;
using Shared;

namespace Application.Branchs.Queries.GetAllDeleted;

public record GetAllDeletedBranchesQuery(
    PageRequest Page
) : IRequest<Result<PageResult<BranchListDto>>>;