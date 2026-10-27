using Application.Interfaces.Repositories.BaseInterfaces;
using Domain.Entities;

namespace Application.Interfaces.Repositories.ServiceInterfaces;

public interface IServiceWriteRepository:IWriteRepository<Service,Guid>
{
    Task<Service?> GetServiceByIdAsync(Guid organId,Guid id, CancellationToken ct);
    Task<bool> ExistByNameAsync(Guid organId,string name, CancellationToken ct);
}