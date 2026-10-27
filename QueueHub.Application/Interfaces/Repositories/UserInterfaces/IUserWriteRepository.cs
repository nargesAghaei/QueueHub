using Application.Interfaces.Repositories.BaseInterfaces;
using Domain.Entities;

namespace Application.Interfaces.Repositories.UserInterfaces;

public interface IUserWriteRepository:IWriteRepository<User, Guid>
{ 
    Task<User?> GetStaffByIdAsync(Guid id,Guid organId,CancellationToken ct);
    Task<User?> GetDeletedStaffByIdAsync(Guid id,Guid organId, CancellationToken ct);
    Task<User?> GetDeletedUserByIdAsync(Guid id, CancellationToken ct);
    Task<bool> ExistByUserNameAsync(Guid id,string userName, CancellationToken ct);
}