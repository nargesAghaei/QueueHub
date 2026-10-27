using Application.Interfaces.Repositories.OrganizationInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Repositories.OrganizationRepo;

public class OrganizationWriteRepository(QueueHubDbContext dbContext)
    : BaseWriteRepository<Organization, Guid>(dbContext), IOrganizationWriteRepository
{
}