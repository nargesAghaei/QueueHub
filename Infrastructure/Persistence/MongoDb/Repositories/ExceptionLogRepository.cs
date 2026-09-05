using Domain.Entities;
using Domain.Interfaces;
using MongoDB.Driver;

namespace Infrastructure.Persistence.MongoDb.Repositories;

public class ExceptionLogReadRepository(MongoContext context):IExceptionLogReadRepository
{
    private IMongoCollection<ExceptionLog> Collection => context.ExceptionLogs;

    public async Task<ExceptionLog?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var method1 = await Collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

        var method2 = await Collection.FindAsync(
            Builders<ExceptionLog>.Filter.Eq(x => x.Id, id),
            cancellationToken: cancellationToken);

        return await method2.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ExceptionLog>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Collection
            .Find(Builders<ExceptionLog>.Filter.Empty)
            .SortByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ExceptionLog>> GetRecentAsync(int count,
        CancellationToken cancellationToken = default)
    {
        return await Collection
            .Find(Builders<ExceptionLog>.Filter.Empty)
            .SortByDescending(x => x.CreatedAt)
            .Limit(count)
            .ToListAsync(cancellationToken);
    }
}