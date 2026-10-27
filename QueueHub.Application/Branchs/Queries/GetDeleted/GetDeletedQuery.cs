using Application.Branchs.DTOs;
using Application.Common.Pagination;
using MediatR;
using Shared;

namespace Application.Branchs.Queries.GetDeleted;

public record GetDeletedQuery(
    Guid Id)
    :IRequest<Result<BranchDto>>;