using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.SqlServer;

public class QueueHubDbContext(DbContextOptions<QueueHubDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}