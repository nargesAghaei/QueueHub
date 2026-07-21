namespace Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity, CancellationToken cancellationToken);
    void Delete(T entity);
    void Update(T entity);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}