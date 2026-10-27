using Application.Interfaces.Repositories.BaseInterfaces;
using Domain.Entities;

namespace Application.Interfaces.Repositories.OrganizationInterfaces;

public interface IOrganizationWriteRepository : IWriteRepository<Organization, Guid>
{
}