using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.PostgreSql.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class RefreshTokenWriteRepository(QueueHubDbContext dbContext) :BaseWriteRepository<RefreshToken, int>(dbContext),IRefreshTokenWriteRepository
{
    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken) =>
        await DbSet.FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);

    public async Task RevokeAllByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var activeTokens = await DbSet
            .Where(rt => rt.UserId == userId && !rt.IsRevokes)
            .ToListAsync(cancellationToken);

        foreach (var token in activeTokens)
        {
            token.Revoke();
        }
    }
}