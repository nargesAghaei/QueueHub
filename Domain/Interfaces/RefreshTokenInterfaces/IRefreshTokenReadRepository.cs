using Domain.Entities;

namespace Domain.Interfaces;

public interface IRefreshTokenReadRepository:IReadRepository<RefreshToken, int>
{
    Task<List<RefreshToken>> GetActiveTokenByUserIdAsync(Guid userId,CancellationToken cancellationToken);
}