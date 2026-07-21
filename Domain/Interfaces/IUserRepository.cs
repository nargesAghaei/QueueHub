using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository:IRepository<User>
{
    Task<List<User>>  GetAllAsync(CancellationToken cancellationToken);
    Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken, Guid? exceptUserId = null);
    Task<User> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
    Task<bool> ExistsById(Guid id, CancellationToken cancellationToken);
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken); 
}