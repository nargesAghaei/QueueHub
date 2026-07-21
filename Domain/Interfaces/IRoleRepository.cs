using Domain.Entities;

namespace Domain.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(
        string roleName,
        CancellationToken cancellationToken);

    Task<Role?> GetByIdAsync(
        Guid roleId,
        CancellationToken cancellationToken);
}