using System.Collections;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Queue = Domain.Entities.Queue;

namespace Infrastructure.Persistence.PostgreSql.EfCore;

public class QueueHubDbContext(DbContextOptions<QueueHubDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<OrganizationCategory> OrganizationCategories { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Queue> Queues { get; set; }
    public DbSet<BranchService> BranchServices { get; set; }
    public DbSet<BranchWorkingHour> BranchWorkingHours { get; set; }
    public DbSet<QueueTicket> QueueTickets { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(QueueHubDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}