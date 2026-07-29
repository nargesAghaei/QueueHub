using Domain.Entities;
using Infrastructure.Persistence.PostgreSql.EfCore;
using Infrastructure.Persistence.SqlServer;

namespace Infrastructure.Persistence;

public class UnitOfWork:IUnitOfWork
{
    private readonly QueueHubDbContext _dbContext;
    public UnitOfWork(QueueHubDbContext dbContext) => _dbContext = dbContext;
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _dbContext.SaveChangesAsync(cancellationToken);
}