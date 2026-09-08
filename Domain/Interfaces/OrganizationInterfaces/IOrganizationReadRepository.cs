using Domain.Entities;

namespace Domain.Interfaces.OrganizationInterfaces;

public interface IOrganizationReadRepository:IReadRepository<Organization,Guid>
{
    Task<List<Organization>>  GetAllOrganizationsAsync(Guid categoryId,Guid? serviceId,CancellationToken cancellationToken = default);
    Task<bool> ExistsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
}