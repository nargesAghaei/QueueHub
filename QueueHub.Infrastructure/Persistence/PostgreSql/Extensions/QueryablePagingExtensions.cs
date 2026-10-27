using Application.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.PostgreSql.Extensions;

public static class QueryablePagingExtensions
{
    public static async Task<PageResult<T>> ToPageResultAsync<T>(
        this IQueryable<T> query, PageRequest page, CancellationToken ct=default)
    {
        var pageNumber = Math.Max(page.PageNumber, 1);
        var pageSize = Math.Clamp(page.PageSize, 1, PageRequest.MaxPageSize);
        
        var totalCount = await query.CountAsync(ct);

        var skip = (pageNumber - 1L) * pageSize;

        List<T> items = skip >= totalCount
            ? []
            : await query.Skip((int)skip).Take(pageSize).ToListAsync(ct);

        return new PageResult<T>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}