namespace Domain.Interfaces;

public interface IWriteRepository<T, in TId> where T : class, IEntity<TId>
{
    Task AddAsync(T entity, CancellationToken cancellationToken);
    void Delete(T entity);
    void Update(T entity);
    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken);
}