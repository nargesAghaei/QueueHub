using Domain.Exceptions;

namespace Domain.Entities;

public class Role:BaseEntity<int>
{
    public string Name { get; private set; } = null!;

    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();
    private static readonly DateTime SeedDate = 
        new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private Role() { }

    public static Role Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Role name is required.");

        return new Role
        {
            Name = name.Trim(),
            CreatedAt = DateTime.UtcNow
        };
    }
    public static Role Seed(
        int id,
        string name)
    {
        return new Role
        {
            Id = id,
            Name = name,
            CreatedAt =SeedDate
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