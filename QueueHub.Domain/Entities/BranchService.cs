namespace Domain.Entities;

public class BranchService : BaseEntity<Guid>
{
    public Guid BranchId { get; private set; }
    public Branch Branch { get; private set; } = null!;

    public Guid ServiceId { get; private set; }
    public Service Service { get; private set; } = null!;

    private readonly List<Queue> _queues = new();
    public IReadOnlyCollection<Queue> Queues => _queues.AsReadOnly();
    
    public bool IsActive { get; private set; }

    private BranchService() { }

    public static BranchService Create(
        Guid branchId,
        Guid serviceId,
        Guid createdBy)
    {
        return new BranchService
        {
            Id = Guid.NewGuid(),
            BranchId = branchId,
            ServiceId = serviceId,
            IsActive = true,
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
}