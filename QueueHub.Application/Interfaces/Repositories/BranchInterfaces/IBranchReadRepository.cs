using Application.Branchs.DTOs;
using Application.Branchs.Filters;
using Application.Common.Pagination;
using Application.Interfaces.Repositories.BaseInterfaces;
using Domain.Entities;

namespace Application.Interfaces.Repositories.BranchInterfaces;

public interface IBranchReadRepository:IReadRepository<Branch,Guid>
{
    Task<PageResult<Branch>> GetAllBranchesAsync(
        BranchFilter filter, PageRequest page, CancellationToken ct = default);

    Task<Branch?> GetBranchByIdAsync(Guid organId, Guid id, CancellationToken ct);
    Task<PageResult<Branch>> GetAllDeletedBranchesAsync(Guid organId,PageRequest page, CancellationToken ct);
    Task<Branch?> GetDeletedBranchAsync(Guid organId,Guid id, CancellationToken ct);
}