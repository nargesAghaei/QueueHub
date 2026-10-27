using Domain.Interfaces;

namespace Domain.Entities;

public class BranchWorkingHour : BaseEntity<Guid>, ISoftDeletable
{
    public Guid BranchId { get; private set; }
    public Branch Branch { get; private set; } = null!;

    public DayOfWeek DayOfWeek { get; private set; }
    public TimeOnly? StartTime { get; private set; }
    public TimeOnly? EndTime { get; private set; }

    public bool IsClosed => StartTime is null || EndTime is null;
    public bool IsDeleted { get; private set; }

    private BranchWorkingHour() { }

    public static BranchWorkingHour Create(
        Guid branchId,
        DayOfWeek dayOfWeek,
        TimeOnly? startTime,
        TimeOnly? endTime,
        Guid createdBy)
    {

        return new BranchWorkingHour
        {
            Id = Guid.NewGuid(),
            BranchId = branchId,
            DayOfWeek = dayOfWeek,
            StartTime = startTime,
            EndTime = endTime,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };
    }

    public void Update(
        TimeOnly? startTime,
        TimeOnly? endTime,
        Guid updatedBy)
    {
        StartTime = startTime;
        EndTime = endTime;
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