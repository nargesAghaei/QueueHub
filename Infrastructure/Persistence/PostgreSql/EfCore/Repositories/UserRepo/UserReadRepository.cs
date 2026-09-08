using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.PostgreSql.EfCore;
using Infrastructure.Persistence.PostgreSql.EfCore.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class UserReadRepository(QueueHubDbContext dbContext)
    : BaseReadRepository<User, Guid>(dbContext), IUserReadRepository
{
    private IUserReadRepository _userReadRepositoryImplementation;

    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken) =>
        await dbContext.Users
            .Include(u => u.ActiveRole)
            .FirstOrDefaultAsync(u => u.UserName.Value == userName && !u.IsDeleted, cancellationToken: cancellationToken);

    public async Task<bool> ExistsById(Guid id, CancellationToken cancellationToken)=>
        await DbSet.AnyAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);

    public async Task<List<User>> GetAllUserAsync(bool isDeleted,int? roleId = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Where(u => u.IsDeleted == isDeleted);

        if (roleId.HasValue)
        {
            query = query.Where(u =>
                u.UserRoles.Any(ur => ur.RoleId == roleId.Value));
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken,
        Guid? exceptUserId = null)=>
        await DbSet.AnyAsync(u =>
                u.UserName.Value == userName &&
                !u.IsDeleted &&
                (exceptUserId == null || u.Id != exceptUserId),
            cancellationToken);

    public async Task<User?> GetByIdWithRolesAsync(Guid id, CancellationToken cancellationToken)=>
        await DbSet
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);

    public async Task<List<User>> GetByRoleAsync(int roleId, CancellationToken cancellationToken = default)=>
        await DbSet
            .Where(u =>
                !u.IsDeleted &&
                u.UserRoles.Any(ur => ur.RoleId == roleId))
            .ToListAsync(cancellationToken);
    
    
    
    public async Task<List<User>> GetStaffAsync(
        Guid organizationId,
        Guid? branchId = null,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Where(u =>
                !u.IsDeleted &&
                u.OrganizationId == organizationId &&
                u.UserRoles.Any(ur => ur.Role.Name == RoleNames.Staff));
    
        if (branchId.HasValue)
        {
            query = query.Where(u =>
                u.BranchId == branchId.Value);
        }
    
        return await query.ToListAsync(cancellationToken);
    }
}