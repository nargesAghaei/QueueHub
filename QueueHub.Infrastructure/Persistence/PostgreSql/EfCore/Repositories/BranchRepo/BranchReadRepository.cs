using Application.Branchs.DTOs;
using Application.Branchs.Filters;
using Application.Common.Pagination;
using Application.Interfaces.Repositories.BranchInterfaces;
using Domain.Common.Time;
using Domain.Entities;
using Infrastructure.Persistence.PostgreSql.EfCore.Repositories.BaseRepo;
using Infrastructure.Persistence.PostgreSql.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Repositories.BranchRepo;

public class BranchReadRepository(QueueHubDbContext dbContext)
    : BaseReadRepository<Branch, Guid>(dbContext), IBranchReadRepository
{
    public Task<PageResult<Branch>> GetAllBranchesAsync(
        BranchFilter filter,
        PageRequest page,
        CancellationToken ct = default)
    {
        var query = ApplyFilters(
            DbSet
                .AsNoTracking()
                .Include(b => b.City)
                .ThenInclude(c => c.Province)
                .Include(b => b.WorkingHours),
            filter);

        return query
            .OrderBy(b => b.Name.Value)
            .ThenBy(b => b.Id)
            .AsSplitQuery()
            .ToPageResultAsync(page, ct);
    }

    public async Task<Branch?> GetBranchByIdAsync(
        Guid organId,
        Guid id,
        CancellationToken ct) =>
        await DbSet.FirstOrDefaultAsync(b =>
                b.Id == id &&
                b.OrganizationId == organId,
            ct);

    private static IQueryable<Branch> ApplyFilters(
        IQueryable<Branch> query,
        BranchFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var pattern = $"%{EscapeLike(filter.Search.Trim())}%";
            query = query.Where(b => EF.Functions.ILike(b.Name.Value, pattern));
        }

        if (filter.OrganId is not null)
            query = query.Where(b => b.OrganizationId == filter.OrganId.Value);

        if (filter.CityId is not null)
            query = query.Where(b => b.CityId == filter.CityId.Value);

        if (filter.ServiceId is not null)
            query = query.Where(b =>
                b.BranchServices.Any(bs => bs.ServiceId == filter.ServiceId.Value));

        if (filter.IsActive is not null)
            query = query.Where(b => b.IsActive == filter.IsActive.Value);

        if (filter.IsOpen is not null)
        {
            var now = TehranClock.Now;
            var today = now.DayOfWeek;
            var time = TimeOnly.FromDateTime(now);

            query = filter.IsOpen.Value
                ? query.Where(b => b.IsActive && b.WorkingHours.Any(w =>
                    !w.IsDeleted &&
                    w.DayOfWeek == today &&
                    w.StartTime <= time && w.EndTime > time))
                : query.Where(b => !b.IsActive || !b.WorkingHours.Any(w =>
                    !w.IsDeleted &&
                    w.DayOfWeek == today &&
                    w.StartTime <= time && w.EndTime > time));
        }

        return query;
    }

    public async Task<PageResult<Branch>> GetAllDeletedBranchesAsync(
        Guid organId,
        PageRequest page,
        CancellationToken ct)
    {
        var query = DbSet
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(b => b.City)
            .ThenInclude(c => c.Province)
            .Where(b =>
                b.IsDeleted &&
                b.OrganizationId == organId)
            .OrderBy(b => b.Name.Value)
            .ThenBy(b => b.Id);

        return await query.ToPageResultAsync(page, ct);
    }

    public async Task<Branch?> GetDeletedBranchAsync(
        Guid organId,
        Guid id,
        CancellationToken ct) =>
        await DbSet
            .IgnoreQueryFilters()
            .Include(b => b.City)
            .ThenInclude(c => c.Province)
            .FirstOrDefaultAsync(b =>
                    b.Id == id &&
                    b.OrganizationId == organId &&
                    b.IsDeleted,
                ct);

    private static string EscapeLike(string value) =>
        value.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_");
}