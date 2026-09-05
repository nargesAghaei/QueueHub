using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserReadRepository:IReadRepository<User, Guid>
{
    Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken, Guid? exceptUserId = null);
    Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
    Task<bool> ExistsById(Guid id, CancellationToken cancellationToken);
    Task<User?> GetByIdWithRolesAsync(Guid id, CancellationToken cancellationToken);
}