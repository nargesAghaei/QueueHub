using Application.Interfaces.Repositories.BaseInterfaces;
using Domain.Entities;

namespace Application.Interfaces.Repositories.RefreshTokenInterfaces;

public interface IRefreshTokenReadRepository:IReadRepository<RefreshToken, int>
{
    Task<List<RefreshToken>> GetActiveTokenByUserIdAsync(Guid userId,CancellationToken cancellationToken);
}