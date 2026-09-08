using Domain.Interfaces.OrganizationInterfaces;
using Domain.Entities;
using Infrastructure.Persistence.PostgreSql.EfCore.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Repositories.OrganizationRepo;

public class OrganizationReadRepository(QueueHubDbContext dbContext)
    :BaseReadRepository<Organization,Guid>(dbContext),IOrganizationReadRepository
{
    public async Task<List<Organization>> GetAllOrganizationsAsync(Guid categoryId, Guid? serviceId,
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.Organizations
            .Where(x => !x.IsDeleted && x.OrganizationCategoryId == categoryId)
            .AsQueryable();

        if (serviceId != default)
        {
            query = query.Where(x => x.Services.Any
                (s => s.Id == serviceId && !s.IsDeleted));
        }

        return await query.ToListAsync();
    }
    
    public async Task<bool> ExistsByCategoryIdAsync(Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Organizations
            .AnyAsync(x => x.OrganizationCategoryId == categoryId && !x.IsDeleted,
                cancellationToken);
    }
}