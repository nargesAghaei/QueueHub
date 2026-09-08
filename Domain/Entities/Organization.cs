using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.OrganizationValueObjects;
using Domain.ValueObjects.UserValueObjects;

namespace Domain.Entities;

public class Organization : BaseEntity<Guid>,ISoftDeletable
{
    public Name Name { get; private set; } = null!;
    public string OrganizationCode { get; private set; } = null!;
    public Description Description { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string? Website { get; private set; }
    public Address? Address { get; private set; }
    public string? City { get; private set; }
    public string? LogoUrl { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }
    

    public Guid OrganizationCategoryId { get; private set; }
    public OrganizationCategory OrganizationCategory { get; private set; } = null!;
    public User Manager { get; private set; } = null!;
    private readonly List<Branch> _branches = new();
    public IReadOnlyCollection<Branch> Branches => _branches.AsReadOnly();
    private readonly List<Service> _services = new();
    public IReadOnlyCollection<Service> Services => _services.AsReadOnly();
    
    private Organization(){ }

    public static Organization Create(
        Name name,
        string organizationCode,
        Description description,
        PhoneNumber phoneNumber,
        Email email,
        Guid createdBy,
        string? website,
        Address? address,
        string? city,
        string? logoUrl,
        Guid categoryId)
    {
        return new Organization()
        {
            Id = Guid.NewGuid(),
            Name = name,
            OrganizationCode = organizationCode,
            Description = description,
            PhoneNumber = phoneNumber,
            Email = email,
            Website = website,
            Address = address,
            City = city,
            LogoUrl = logoUrl,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false,
            CreatedBy = createdBy
        };

    }
    
    
    public void Update(
        Name name,
        string organizationCode,
        Description description,
        PhoneNumber phoneNumber,
        Email email,
        Guid updatedBy,
        string? website,
        Address? address,
        string? city,
        string? logoUrl)
    {
        Name = name;
        OrganizationCode= organizationCode;
        Description = description;
        PhoneNumber = phoneNumber;
        Email = email;
        Website = website;
        Address = address;
        City= city;
        LogoUrl = logoUrl;
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SoftDelete(Guid updatedBy)
    {
        IsDeleted = true;
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
    
    public void Restore(Guid updatedBy)
    {
        IsDeleted = false;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}