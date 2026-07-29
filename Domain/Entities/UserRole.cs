namespace Domain.Entities;

public class UserRole:BaseEntity<int>
{
    public Guid UserId { get; private set; }
    public int RoleId { get; private set; }
    public User User { get; private set; } = null!;
    public Role Role { get; private set; } = null!;
    public bool IsDeleted { get; set; }

    internal UserRole(Guid userId, int roleId)
    {
        UserId = userId;
        RoleId = roleId;
        IsDeleted = false;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        
    }
    public void Delete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
    private UserRole() { }
}