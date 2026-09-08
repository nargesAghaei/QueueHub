using Domain.Exceptions;
using Domain.ValueObjects.OrganizationValueObjects;

namespace Domain.Entities;

public class OrganizationCategory:BaseEntity<Guid>
{
    public Name Name { get; private set; } = null!;
    public Description? Description { get; private set; }
    public bool IsDeleted { get; private set; }
    
    private readonly List<Organization> _organizations = new();

    public IReadOnlyCollection<Organization> Organizations =>
        _organizations.AsReadOnly();


    private OrganizationCategory() { }
    
    public static OrganizationCategory Create(
        Name name,
        Description? description,
        Guid createdBy)
    {
        return new OrganizationCategory
        {
            Name = name,
            Description = description,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };
    }


    public void Update(
        Name name,
        Description? description)
    {
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }


    public void SoftDelete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }


    public void Restore()
    {
        IsDeleted = false;
        UpdatedAt = DateTime.UtcNow;
    }
}