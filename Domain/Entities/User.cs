using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;

namespace Domain.Entities;

public class User : BaseEntity<Guid>,ISoftDeletable
{
    public FirstName FirstName { get; private set; } = null!;
    public Lastname LastName { get; private set; } = null!;
    public UserName UserName { get; private set; } = null!;
    public PasswordHash PasswordHash { get; private set; } = null!;
    public Email? Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public string? ProfileImageUrl { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public int? ActiveRoleId { get; private set; }
    public Role? ActiveRole { get; private set; }
    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();
    public Guid? OrganizationId { get; private set; }

    private User() { }

    public static User RegisterCitizen(
        FirstName firstName,
        Lastname lastName,
        UserName userName,
        PasswordHash passwordHash,
        PhoneNumber phoneNumber,
        Email? email = null)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            UserName = userName,
            PasswordHash = passwordHash,
            PhoneNumber = phoneNumber,
            Email = email,
            OrganizationId = null,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
    }
    
    public static User RegisterManager(
        FirstName firstName,
        Lastname lastName,
        UserName userName,
        PasswordHash passwordHash,
        PhoneNumber phoneNumber,
        Guid organizationId,
        Email? email = null)
    {
        if (organizationId == Guid.Empty)
            throw new DomainException("Organization is required for a manager.");

        return new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            UserName = userName,
            PasswordHash = passwordHash,
            PhoneNumber = phoneNumber,
            Email = email,
            OrganizationId = organizationId,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static User RegisterStaff(
        FirstName firstName,
        Lastname lastName,
        UserName userName,
        PasswordHash passwordHash,
        PhoneNumber phoneNumber,
        Guid? organizationId,
        Guid createdByManagerId,
        Email? email = null)
    {
        if (organizationId == Guid.Empty)
            throw new DomainException("Organization is required for staff.");

        return new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            UserName = userName,
            PasswordHash = passwordHash,
            PhoneNumber = phoneNumber,
            Email = email,
            OrganizationId = organizationId,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdByManagerId
        };
    }

    public void UpdateProfile(
        FirstName firstName,
        Lastname lastName,
        PhoneNumber phoneNumber,
        Email email,
        string? profileImageUrl)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber= phoneNumber;
        Email = email;
        ProfileImageUrl = profileImageUrl;
    }
    
    public void UpdatePassword(PasswordHash newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateUserName(UserName newUserName)
    {
        UserName = newUserName;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
        foreach (var userRole in _userRoles)
        {
            userRole.Delete();
        }
    }

    public void AssignRole(Role role)
    {
        if (_userRoles.Any(ur => ur.RoleId == role.Id))
            throw new DomainException("User already has this role.");

        _userRoles.Add(new UserRole(Id, role.Id));
        ActiveRoleId = role.Id;
    }

    public void SwitchRole(int roleId)
    {
        if (_userRoles.Any(ur => ur.RoleId == roleId))
            throw new InvalidOperationException("User does not have this role");

        ActiveRoleId = roleId;
    }
    
    public void RemoveRole(int roleId)
    {
        var userRole = _userRoles.FirstOrDefault(ur => ur.RoleId == roleId);

        if (userRole is null)
            throw new DomainException("User does not have this role.");
        _userRoles.Remove(userRole);
    }
}