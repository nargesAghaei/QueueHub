using Domain.Entities;

namespace Domain.Interfaces;

public interface IRefreshTokenWriteRepository:IWriteRepository<RefreshToken, int>
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);
    Task RevokeAllByUserIdAsync(Guid userId,CancellationToken cancellationToken);
}