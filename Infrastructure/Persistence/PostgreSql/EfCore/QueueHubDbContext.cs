using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.PostgreSql.EfCore;

public class QueueHubDbContext(DbContextOptions<QueueHubDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(QueueHubDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}