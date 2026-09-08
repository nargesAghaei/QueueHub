using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserReadRepository:IReadRepository<User, Guid>
{
    Task<List<User>> GetStaffAsync(Guid organizationId, Guid? branchId = null, CancellationToken cancellationToken = default);
    Task<List<User>> GetAllUserAsync(bool isDeleted,int? roleId = null,CancellationToken cancellationToken = default);
    Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken, Guid? exceptUserId = null);
    Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
    Task<bool> ExistsById(Guid id, CancellationToken cancellationToken);
    Task<User?> GetByIdWithRolesAsync(Guid id, CancellationToken cancellationToken);
    Task<List<User>> GetByRoleAsync(int roleId, CancellationToken cancellationToken = default);
}