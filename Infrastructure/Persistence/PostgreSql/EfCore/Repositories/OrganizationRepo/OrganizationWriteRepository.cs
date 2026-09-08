using Domain.Entities;
using Domain.Interfaces.OrganizationInterfaces;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Repositories.OrganizationRepo;

public class OrganizationWriteRepository(QueueHubDbContext dbContext)
    :BaseWriteRepository<Organization,Guid>(dbContext),IOrganizationWriteRepository
{
    
}