using Domain.Interfaces;
using Domain.ValueObjects.OrganizationValueObjects;

namespace Domain.Entities;

public class Service : BaseEntity<Guid>, ISoftDeletable
{
    public Name Name { get; private set; } = null!;
    public Description Description { get; private set; } = null!;
    public int EstimatedDurationMinutes { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    public Guid OrganizationId { get; private set; }
    public Organization Organization { get; private set; } = null!;

    private readonly List<BranchService> _branchServices = new();
    public IReadOnlyCollection<BranchService> BranchServices => _branchServices.AsReadOnly();

    private Service() { }

    public static Service Create(
        Name name,
        Description description,
        int estimatedDurationMinutes,
        Guid organizationId,
        Guid createdBy)
    {
        return new Service
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            EstimatedDurationMinutes = estimatedDurationMinutes,
            OrganizationId = organizationId,
            IsActive = true,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };
    }

    public void Update(
        Name name,
        Description description,
        int estimatedDurationMinutes,
        Guid updatedBy)
    {
        Name = name;
        Description = description;
        EstimatedDurationMinutes = estimatedDurationMinutes;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
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