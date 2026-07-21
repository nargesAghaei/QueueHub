using Domain.Exceptions;

namespace Domain.Entities;

public class Role:BaseEntity
{
    public string Name { get; private set; } = null!;

    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    private Role() { }

    public static Role Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Role name is required.");

        return new Role
        {
            Guid = Guid.NewGuid(),
            Name = name.Trim(),
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Role name cannot be empty.");

        Name = newName.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
}