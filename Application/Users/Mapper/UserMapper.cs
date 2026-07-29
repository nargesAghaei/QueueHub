using Application.Users.Commands.CreateUser;
using Application.Users.Commands.UpdateUserProfile;
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
            Id = user.Id,
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
            Id = user.Id,
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
}