namespace Domain.Interfaces;

public interface IReadRepository<T, in TId> where T : class, IEntity<TId>
{
    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken);
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
}