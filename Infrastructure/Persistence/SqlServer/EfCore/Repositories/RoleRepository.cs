using Domain.Entities;
using Domain.Interfaces;
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
        Guid roleId,
        CancellationToken cancellationToken)
    {
        return await context.Roles
            .FirstOrDefaultAsync(
                x => x.Guid == roleId,
                cancellationToken);
    }
}