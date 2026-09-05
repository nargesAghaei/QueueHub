using Domain.Entities;
using Domain.Interfaces;
using MongoDB.Driver;

namespace Infrastructure.Persistence.MongoDb.Repositories;

public class ExceptionLogWriteRepository(MongoContext context):IExceptionLogWriteRepository
{
    private IMongoCollection<ExceptionLog> Collection => context.ExceptionLogs;

    public Task AddAsync(ExceptionLog log, CancellationToken cancellationToken = default)
        => Collection.InsertOneAsync(log, options: null, cancellationToken);

}