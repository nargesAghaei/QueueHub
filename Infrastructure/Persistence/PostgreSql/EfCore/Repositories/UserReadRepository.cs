using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.PostgreSql.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class UserReadRepository(QueueHubDbContext dbContext)
    : BaseReadRepository<User, Guid>(dbContext), IUserReadRepository
{
    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.UserName.Value == userName && !u.IsDeleted, cancellationToken);
    }

    public async Task<bool> ExistsById(Guid id, CancellationToken cancellationToken)
    {
        return await DbSet.AnyAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);
    }
    
    public async Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken,
        Guid? exceptUserId = null)
    {
        return await DbSet.AnyAsync(u =>
                u.UserName.Value == userName &&
                !u.IsDeleted &&
                (exceptUserId == null || u.Id != exceptUserId),
            cancellationToken);
    }

    public async Task<User?> GetByIdWithRolesAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbSet
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);
    }
    
}