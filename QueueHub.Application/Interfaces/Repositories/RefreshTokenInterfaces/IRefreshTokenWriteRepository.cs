using Application.Interfaces.Repositories.BaseInterfaces;
using Domain.Entities;

namespace Application.Interfaces.Repositories.RefreshTokenInterfaces;

public interface IRefreshTokenWriteRepository:IWriteRepository<RefreshToken, int>
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);
    Task RevokeAllByUserIdAsync(Guid userId,CancellationToken cancellationToken);
}