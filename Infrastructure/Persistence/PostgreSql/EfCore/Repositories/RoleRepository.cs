using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.PostgreSql.EfCore;
using Infrastructure.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class RoleRepository(QueueHubDbContext context) : IRoleRepository
{
    public async Task<Role?> GetByNameAsync(
        string roleName,
        CancellationToken cancellationToken)
    {
        return await context.Roles
            .FirstOrDefaultAsync(
                x => x.Name == roleName,
                cancellationToken);
    }


    public async Task<Role?> GetByIdAsync(
        int roleId,
        CancellationToken cancellationToken)
    {
        return await context.Roles
            .FirstOrDefaultAsync(
                x => x.Id == roleId,
                cancellationToken);
    }
}