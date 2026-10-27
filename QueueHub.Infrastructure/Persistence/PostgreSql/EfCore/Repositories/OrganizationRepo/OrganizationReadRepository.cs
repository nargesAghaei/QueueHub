using Application.Common.Pagination;
using Application.Interfaces.Repositories.OrganizationInterfaces;
using Application.Organizations.Filters;
using Domain.Entities;
using Infrastructure.Persistence.PostgreSql.EfCore.Repositories.BaseRepo;
using Infrastructure.Persistence.PostgreSql.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Repositories.OrganizationRepo;

public class OrganizationReadRepository(QueueHubDbContext dbContext)
    : BaseReadRepository<Organization, Guid>(dbContext), IOrganizationReadRepository
{
    
    public async Task<PageResult<Organization>> GetAllOrganizationsAsync(
        OrganizationFilter filter,
        PageRequest page,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .AsNoTracking()
            .AsQueryable();

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(o =>
                o.OrganizationCategoryId == filter.CategoryId.Value);
        }

        if (filter.ServiceId.HasValue)
        {
            query = query.Where(o =>
                o.Branches.Any(b =>
                    b.BranchServices.Any(s =>
                        s.Id == filter.ServiceId.Value)));
        }

        return await query.ToPageResultAsync(page,cancellationToken);
    }


    public async Task<Organization?> GetDeletedAsync(
        Guid? id,
        CancellationToken cancellationToken = default) =>
        await DbSet.IgnoreQueryFilters()
            .FirstOrDefaultAsync(o => 
                o.Id == id &&
                o.IsDeleted,
                cancellationToken);

    
    public async Task<PageResult<Organization>> GetAllDeletedAsync(
        Guid? categoryId,
        PageRequest page,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(o => o.IsDeleted);

        if (categoryId.HasValue)
        {
            query = query.Where(o =>
                o.OrganizationCategoryId == categoryId.Value);
        }

        return await query.ToPageResultAsync(
            page,
            cancellationToken);
    }

}