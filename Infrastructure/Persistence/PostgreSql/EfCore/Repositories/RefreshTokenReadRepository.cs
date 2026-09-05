using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.PostgreSql.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class RefreshTokenReadRepository(QueueHubDbContext dbContext) :BaseReadRepository<RefreshToken, int>(dbContext),IRefreshTokenReadRepository
{
    public async Task<List<RefreshToken>> GetActiveTokenByUserIdAsync(Guid userId, CancellationToken cancellationToken) =>
        await DbSet.AsNoTracking()
            .Where(rt => rt.UserId == userId && rt.ExpireAt >= DateTime.UtcNow)
            .ToListAsync(cancellationToken);
}