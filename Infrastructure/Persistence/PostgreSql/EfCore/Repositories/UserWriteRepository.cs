using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.PostgreSql.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class UserWriteRepository(QueueHubDbContext dbContext)
    : BaseWriteRepository<User, Guid>(dbContext), IUserWriteRepository
{
    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await DbSet.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        if (user is null)
            return false;

        user.SoftDelete();
        return true;
    }
}