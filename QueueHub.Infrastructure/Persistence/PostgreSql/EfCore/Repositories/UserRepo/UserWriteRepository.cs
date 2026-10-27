using Application.Interfaces.Repositories.UserInterfaces;
using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Repositories.UserRepo;

public class UserWriteRepository(QueueHubDbContext dbContext)
    : BaseWriteRepository<User, Guid>(dbContext), IUserWriteRepository
{
    public async Task<User?> GetStaffByIdAsync(
        Guid id,
        Guid organId,
        CancellationToken ct) =>
        await DbSet.FirstOrDefaultAsync(u =>
                u.Id == id &&
                u.OrganizationId == organId &&
                u.UserRoles.Any(ur =>
                    ur.Role.Name == RoleNames.Staff),
            ct);


    public async Task<User?> GetDeletedStaffByIdAsync(
        Guid id,
        Guid organId,
        CancellationToken ct) =>
        await DbSet.IgnoreQueryFilters()
            .FirstOrDefaultAsync(u =>
                u.IsDeleted &&
                u.OrganizationId == organId &&
                u.UserRoles.Any(ur => ur.Role.Name == RoleNames.Staff) &&
                u.Id == id, ct);

    public async Task<User?> GetDeletedUserByIdAsync(Guid id, CancellationToken ct) =>
        await DbSet.IgnoreQueryFilters()
            .FirstOrDefaultAsync(u =>
                    u.Id == id &&
                    u.IsDeleted
                , ct);

    public async Task<bool> ExistByUserNameAsync(
        Guid id,
        string userName,
        CancellationToken ct) =>
        await DbSet.AnyAsync(u =>
                u.UserName == userName &&
                u.Id != id,
            ct);
}