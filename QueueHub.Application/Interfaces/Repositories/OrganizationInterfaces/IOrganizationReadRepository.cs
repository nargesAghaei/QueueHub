using Application.Common.Pagination;
using Application.Interfaces.Repositories.BaseInterfaces;
using Application.Organizations.Filters;
using Domain.Entities;

namespace Application.Interfaces.Repositories.OrganizationInterfaces;

public interface IOrganizationReadRepository:IReadRepository<Organization,Guid>
{
    Task<PageResult<Organization>>  GetAllOrganizationsAsync(
        OrganizationFilter  filter,
        PageRequest page,
        CancellationToken cancellationToken = default);
    
    Task<Organization?> GetDeletedAsync(
        Guid? id,
        CancellationToken cancellationToken = default);

    Task<PageResult<Organization>> GetAllDeletedAsync(
        Guid? categoryId,
        PageRequest page,
        CancellationToken cancellationToken = default);
}