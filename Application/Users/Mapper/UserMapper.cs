using Application.Users.Commands.CreateUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.DTOs;
using Domain.Entities;
using Domain.ValueObjects.UserValueObjects;

namespace Application.Users.Mapper;

public static class UserMapper
{
    public static UserListDto ToUserListDto(this User user)
    {
        return new UserListDto
        {
            Id = user.Guid,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email?.Value,
            PhoneNumber = user.PhoneNumber,
            LastLoginAt = user.LastLoginAt,
        };
    }

    public static List<UserListDto> ToUserListDto(this IEnumerable<User> users)
    {
        return users.Select(x => x.ToUserListDto()).ToList();
    }
    
    public static UserDetailsDto ToUserDetailsDto(this User user)
    {
        return new UserDetailsDto
        {
            Id = user.Guid,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email?.Value,
            PhoneNumber = user.PhoneNumber,
            ProfileImageUrl = user.ProfileImageUrl,
            LastLoginAt = user.LastLoginAt,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
    
    public static User ToEntity(this CreateCitizenCommand command, string passwordHash)
    {
        return new User
        {
            Guid = Guid.NewGuid(),
            FirstName = new FirstName(command.FirstName),
            LastName = new Lastname(command.LastName),
            UserName = new UserName(command.UserName),
            PhoneNumber = new PhoneNumber(command.PhoneNumber),
            Email = string.IsNullOrWhiteSpace(command.Email)
                ? null
                : new Email(command.Email),

            PasswordHash = passwordHash,
            ProfileImageUrl = command.ProfileImageUrl
        };
    }
    
    public static User ToEntity(this UpdateUserCommand command, string passwordHash)
    {
        return new User
        {
            Guid = command.Id,
            FirstName = new FirstName(command.FirstName),
            LastName = new Lastname(command.LastName),
            UserName = new UserName(command.UserName),
            PhoneNumber = new PhoneNumber(command.PhoneNumber),

            Email = string.IsNullOrWhiteSpace(command.Email)
                ? null
                : new Email(command.Email),

            PasswordHash = passwordHash,
            ProfileImageUrl = command.ProfileImageUrl,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = command.Id
        };
    }
}