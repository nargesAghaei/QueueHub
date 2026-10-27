using Application.Interfaces.Repositories.UserInterfaces;
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

    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken ct) =>
        await dbContext.Users
            .Include(u => u.ActiveRole)
            .FirstOrDefaultAsync(u => u.UserName.Value == userName, ct);

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct)=>
        await DbSet.AnyAsync(u => u.Id == id, ct);

    public async Task<List<User>> GetAllUserAsync(bool withIsDeleted,int? roleId = null
        , CancellationToken ct = default)
    {
        var query = DbSet
            .IgnoreQueryFilters()
            .Where(u => u.IsDeleted == withIsDeleted);

        if (roleId.HasValue)
            query = query.Where(u =>
                u.UserRoles.Any(ur => ur.RoleId == roleId.Value));

        return await query.ToListAsync(ct);
    }

    public async Task<bool> ExistsByUserNameAsync(string userName, CancellationToken ct,
        Guid? exceptUserId = null)=>
        await DbSet.AnyAsync(u =>
                u.UserName.Value == userName &&
                (exceptUserId == null || u.Id != exceptUserId),
            ct);
    
    public async Task<User?> GetByIdWithRolesAsync(Guid id, CancellationToken ct)=>
        await DbSet
            .IgnoreQueryFilters()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    
    public async Task<List<User>> GetByRoleAsync(int roleId, CancellationToken ct = default)=>
        await DbSet.Where(u =>
             u.UserRoles.Any(ur => ur.RoleId == roleId))
            .ToListAsync(ct);

    public async Task<User?> GetStaffByIdAsync(Guid id, CancellationToken ct)=>
        await DbSet.Where(u=>u.Id==id &&
                   u.UserRoles.Any(ur=>ur.Role.Name==RoleNames.Staff))
        .FirstOrDefaultAsync(ct);

    public async Task<List<User>> GetDeletedUserAsync(CancellationToken ct)=>
    await DbSet.IgnoreQueryFilters().Where(u=>u.IsDeleted).ToListAsync(ct);

    public async Task<List<User>> GetDeletedStaffsAsync(Guid branchId, CancellationToken ct) =>
        await DbSet.IgnoreQueryFilters()
            .Where(u => 
            u.IsDeleted &&
            u.UserRoles.Any(ur => ur.Role.Name == RoleNames.Staff))
            .ToListAsync(ct);

    public async Task<List<User>> GetStaffsAsync(Guid organizationId, Guid? branchId = null,
        CancellationToken ct = default)
    {
        
        var query = DbSet.Where(u => u.Branch.OrganizationId == organizationId &&
                                     u.UserRoles.Any(ur => ur.Role.Name == RoleNames.Staff));
        
    
        if (branchId.HasValue)
            query = query.Where(u => u.BranchId == branchId.Value);
    
        
        return await query.ToListAsync(ct);
    }
}