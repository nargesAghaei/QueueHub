using Application.Interfaces.Repositories.BranchInterfaces;
using Domain.Entities;
using Infrastructure.Persistence.PostgreSql.EfCore.Repositories.BaseRepo;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.PostgreSql.EfCore.Repositories.BranchRepo;

public class BranchWriteRepository(QueueHubDbContext dbContext)
    : BaseWriteRepository<Branch, Guid>(dbContext), IBranchWriteRepository
{
    public async Task<bool> DuplicateNameAsync(
        string name,
        Guid organId,
        CancellationToken ct) =>
        DbSet.Any(b =>
            b.Name == name &&
            b.OrganizationId == organId);


    public async Task<Branch?> GetByNameAndIdAsync(
        string name,
        Guid id,
        Guid organId,
        CancellationToken ct = default) =>
        await DbSet.FirstOrDefaultAsync(b =>
                b.OrganizationId == organId &&
                b.Name == name &&
                b.Id != id,
            ct);


    public async Task<Branch?> GetBranchByIdAsync(
        Guid organId,
        Guid branchId,
        CancellationToken ct) =>
        await DbSet.FirstOrDefaultAsync(b =>
                b.Id == branchId &&
                b.OrganizationId == organId, ct
        );


    public async Task<Branch?> GetDeletedBranchByIdAsync(
        Guid branchId,
        Guid organId,
        CancellationToken ct) =>
        await DbSet.IgnoreQueryFilters()
            .FirstOrDefaultAsync(b =>
                    b.Id == branchId &&
                    b.IsDeleted &&
                    b.OrganizationId == organId,
                ct);
}