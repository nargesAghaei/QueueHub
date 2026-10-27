using Application.Interfaces.Repositories.BaseInterfaces;
using Domain.Common;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Repositories.BaseRepo;

public abstract class BaseReadRepository<T, TId>(QueueHubDbContext dbContext)
    :IReadRepository<T, TId>
    where T : class, IEntity<TId>
{
    protected readonly QueueHubDbContext DbContext = dbContext;
    protected readonly DbSet<T> DbSet = dbContext.Set<T>();
    
    public async Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken) =>
        await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken);
                                                                            
    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking().ToListAsync(cancellationToken);
}