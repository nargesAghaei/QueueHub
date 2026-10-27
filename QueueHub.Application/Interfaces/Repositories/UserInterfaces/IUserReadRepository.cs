using Application.Interfaces.Repositories.BaseInterfaces;
using Domain.Entities;

namespace Application.Interfaces.Repositories.UserInterfaces;

public interface IUserReadRepository:IReadRepository<User, Guid>
{
    Task<List<User>> GetStaffsAsync(Guid organizationId, Guid? branchId = null, CancellationToken ct = default);
    Task<List<User>> GetAllUserAsync(bool withIsDeleted,int? roleId = null,CancellationToken ct = default);
    Task<bool> ExistsByUserNameAsync(string userName, CancellationToken ct, Guid? exceptUserId = null);
    Task<User> GetByUserNameAsync(string userName, CancellationToken ct);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct);
    Task<User?> GetByIdWithRolesAsync(Guid id, CancellationToken ct);
    Task<List<User>> GetByRoleAsync(int roleId, CancellationToken ct = default);
    Task<User?> GetStaffByIdAsync(Guid id, CancellationToken ct);
    Task<List<User>> GetDeletedUserAsync(CancellationToken ct);
    Task<List<User>> GetDeletedStaffsAsync(Guid branchId,CancellationToken ct);
}