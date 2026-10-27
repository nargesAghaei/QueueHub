using Domain.Common.Time;
using Domain.Enums;
using Domain.Interfaces;
using Domain.ValueObjects.OrganizationValueObjects;

namespace Domain.Entities;

public class Branch : BaseEntity<Guid>, ISoftDeletable
{
    public Name Name { get; private set; } = null!;
    public Guid CityId { get; private set; }
    public City City { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public Description Description { get; private set; } = null!;
    public PhoneNumber? PhoneNumber { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    public Organization Organization { get; private set; } = null!;
    public Guid OrganizationId { get; private set; }
    private readonly List<User> _staff = new();
    public IReadOnlyCollection<User> Staff => _staff.AsReadOnly();
    private readonly List<BranchService> _branchServices = new();
    public IReadOnlyCollection<BranchService> BranchServices => _branchServices.AsReadOnly();

    private readonly List<BranchWorkingHour> _workingHours = new();
    public IReadOnlyCollection<BranchWorkingHour> WorkingHours => _workingHours.AsReadOnly();

    private Branch()
    {
    }

    public static Branch Create(
        Name name,
        Guid cityId,
        Address address,
        Description description,
        PhoneNumber phoneNumber,
        Guid organizationId,
        Guid createdBy)
    {
        return new Branch
        {
            Id = Guid.NewGuid(),
            Name = name,
            PhoneNumber = phoneNumber,
            Address = address,
            CityId = cityId,
            Description = description,
            OrganizationId = organizationId,
            IsActive = true,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };
    }

    public void Update(
        Name name,
        Guid cityId,
        Address address,
        Description description,
        PhoneNumber phoneNumber,
        Guid updatedBy)
    {
        Name = name;
        PhoneNumber = phoneNumber;
        Description = description;
        Address = address;
        CityId = cityId;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }


    public bool IsOpenAt(DayOfWeek day, TimeOnly time) =>
        IsActive && _workingHours.Any(w =>
            !w.IsDeleted &&
            w.DayOfWeek == day &&
            w.StartTime <= time && w.EndTime > time);

    public bool IsOpenNow()
    {
        var now = TehranClock.Now;
        return IsOpenAt(now.DayOfWeek, TimeOnly.FromDateTime(now));
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