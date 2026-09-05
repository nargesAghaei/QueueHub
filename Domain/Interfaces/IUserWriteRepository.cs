using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserWriteRepository:IWriteRepository<User, Guid>
{
    Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
}