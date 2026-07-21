using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class UserRepository(QueueHubDbContext dbContext)
    : BaseRepository<User>(dbContext), IUserRepository
{
    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        return await DbSet.FirstOrDefaultAsync(u => u.UserName.Value == userName, cancellationToken);
    }

    public async Task<bool> ExistsById(Guid id, CancellationToken cancellationToken)
    {
        return await DbSet.AnyAsync(u => u.Guid == id, cancellationToken);
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await DbSet.ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken,
        Guid? exceptUserId = null)
    {
        return await DbSet.AnyAsync(u =>
                u.UserName.Value == userName &&
                (exceptUserId == null || u.Guid != exceptUserId),
            cancellationToken);
    }

    public async Task<User?> GetByIdWithRolesAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbSet
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Guid == id, cancellationToken);
    }
    
    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await DbSet.FirstOrDefaultAsync(u => u.Guid == id, cancellationToken);

        if (user is null)
            return false;

        user.SoftDelete();
        return true;
    }
}