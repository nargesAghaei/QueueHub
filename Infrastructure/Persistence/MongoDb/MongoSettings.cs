namespace Infrastructure.Persistence.MongoDb;

public class MongoSettings
{
    public string ConnectionString { get; set; } = "mongodb://localhost:27017";
    public string Database { get; set; } = "ShopDb";
    public string ExceptionLogsCollection { get; set; } = "exception_logs";
}