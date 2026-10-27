using Application.Branchs.Filters;
using Application.Common.Pagination;
using Application.Interfaces.Repositories.BaseInterfaces;
using Application.Services.Filters;
using Domain.Entities;

namespace Application.Interfaces.Repositories.ServiceInterfaces;

public interface IServiceReadRepository:IReadRepository<Service,Guid>
{
    Task<PageResult<Service>> GetAllServicesAsync(
        ServiceFilters filter, PageRequest page, CancellationToken ct = default);

    Task<Service?> GetServiceByIdAsync(Guid organId,Guid id, CancellationToken ct);
    Task<bool> ExistByNameAsync(string name,Guid organId, CancellationToken ct);
}