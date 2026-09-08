using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.OrganizationValueObjects;

namespace Domain.Entities;

public class Queue : BaseEntity<Guid>, ISoftDeletable
{
    public Guid BranchServiceId { get; private set; }
    public BranchService BranchService { get; private set; } = null!;
    public DateOnly QueueDate { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    private Queue() { }

    public static Queue Create(
        Guid branchServiceId,
        DateOnly queueDate,
        Guid createdBy)
    {
        if (branchServiceId == Guid.Empty)
            throw new DomainException(
                "Branch service is required.");

        return new Queue
        {
            Id = Guid.NewGuid(),
            BranchServiceId = branchServiceId,
            QueueDate = queueDate,
            IsActive = true,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };
    }

    public void Activate(Guid updatedBy)
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void Deactivate(Guid updatedBy)
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void SoftDelete(Guid updatedBy)
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void Restore(Guid updatedBy)
    {
        IsDeleted = false;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}