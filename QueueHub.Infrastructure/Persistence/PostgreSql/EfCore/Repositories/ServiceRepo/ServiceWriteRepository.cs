using Application.Interfaces.Repositories.ServiceInterfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Repositories.ServiceRepo;

public class ServiceWriteRepository(QueueHubDbContext dbContext)
    : BaseWriteRepository<Service, Guid>(dbContext), IServiceWriteRepository
{
    public async Task<Service?> GetServiceByIdAsync(
        Guid organId,
        Guid id,
        CancellationToken ct) =>
        await DbSet.FirstOrDefaultAsync(s =>
                s.Id == id &&
                s.OrganizationId == organId,
            ct);

    
    public async Task<bool> ExistByNameAsync(
        Guid organId,
        string name,
        CancellationToken ct) =>
        await DbSet.AnyAsync(s =>
                s.Name == name &&
                s.OrganizationId == organId,
            ct);
}