using Application.Interfaces.Repositories.BaseInterfaces;
using Domain.Entities;

namespace Application.Interfaces.Repositories.BranchInterfaces;

public interface IBranchWriteRepository : IWriteRepository<Domain.Entities.Branch, Guid>
{
    Task<bool> DuplicateNameAsync(string name, Guid organId, CancellationToken ct);
    Task<Branch?> GetByNameAndIdAsync(string name,Guid id,Guid organId, CancellationToken ct);
    Task<Branch?> GetBranchByIdAsync(Guid organId, Guid branchId, CancellationToken ct);
    Task<Branch?> GetDeletedBranchByIdAsync(Guid branchId,Guid organId, CancellationToken ct);
}