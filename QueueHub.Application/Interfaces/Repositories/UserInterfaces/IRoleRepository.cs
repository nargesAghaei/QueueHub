using Domain.Entities;

namespace Application.Interfaces.Repositories.UserInterfaces;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(
        string roleName,
        CancellationToken cancellationToken);

    Task<Role?> GetByIdAsync(
        int roleId,
        CancellationToken cancellationToken);
}