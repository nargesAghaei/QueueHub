using Domain.Entities;

namespace Domain.Interfaces;

public interface IExceptionLogReadRepository
{
    Task<ExceptionLog?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ExceptionLog>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ExceptionLog>> GetRecentAsync(int count, CancellationToken cancellationToken = default);
}