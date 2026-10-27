using Application.Common.Pagination;
using Application.Interfaces.Repositories.ServiceInterfaces;
using Application.Services.Filters;
using Domain.Entities;
using Infrastructure.Persistence.PostgreSql.EfCore.Repositories.BaseRepo;
using Infrastructure.Persistence.PostgreSql.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Repositories.ServiceRepo;

public class ServiceReadRepository(QueueHubDbContext dbContext)
    : BaseReadRepository<Service, Guid>(dbContext), IServiceReadRepository
{
    public async Task<PageResult<Service>> GetAllServicesAsync(
        ServiceFilters filter,
        PageRequest page,
        CancellationToken ct = default) =>
        await ApplyFilters(DbSet.AsNoTracking(), filter)
            .ToPageResultAsync(page, ct);

    private static IQueryable<Service> ApplyFilters(
        IQueryable<Service> query,
        ServiceFilters filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(b => b.Name.value.Contains(filter.Search));


        if (filter.OrganId is not null)
            query = query.Where(b => b.OrganizationId == filter.OrganId);


        if (filter.BranchId is not null)
            query = query.Where(b => b.BranchServices
                .Any(bs => bs.BranchId == filter.BranchId));


        if (filter.IsActive is not null)
            query = query.Where(b => b.IsActive == filter.IsActive.Value);


        return query;
    }


    public async Task<Service?> GetServiceByIdAsync(
        Guid organId,
        Guid id,
        CancellationToken ct) =>
        await DbSet.FirstOrDefaultAsync(s =>
                s.Id == id &&
                s.OrganizationId == organId,
            ct);

    
    
    public async Task<bool> ExistByNameAsync(
        string name,
        Guid organId,
        CancellationToken ct) =>
        await DbSet.AnyAsync(s =>
                s.Name == name &&
                s.OrganizationId == organId,
            ct);
}