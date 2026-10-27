using Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Persistence.MongoDb;

public class MongoContext
{
    private readonly MongoSettings _settings;

    public MongoContext(IOptions<MongoSettings> settings)
    {
        _settings = settings.Value;
        var client = new MongoClient(_settings.ConnectionString);
        Database = client.GetDatabase(_settings.Database);
    }

    public IMongoDatabase Database { get; }

    public IMongoCollection<ExceptionLog> ExceptionLogs =>
        Database.GetCollection<ExceptionLog>(_settings.ExceptionLogsCollection);
}
