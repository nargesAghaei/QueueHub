using Domain.Entities;

namespace Application.Interfaces.Repositories.ExceptionLogInterfaces;

public interface IExceptionLogWriteRepository
{
    Task AddAsync(ExceptionLog log, CancellationToken cancellationToken = default);
}