using Domain.Entities;

namespace Domain.Interfaces.OrganizationInterfaces;

public interface IOrganizationWriteRepository:IWriteRepository<Organization,Guid>
{ }