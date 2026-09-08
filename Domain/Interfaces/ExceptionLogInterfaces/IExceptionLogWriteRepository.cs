using Domain.Entities;

namespace Domain.Interfaces;

public interface IExceptionLogWriteRepository
{
    Task AddAsync(ExceptionLog log, CancellationToken cancellationToken = default);
}