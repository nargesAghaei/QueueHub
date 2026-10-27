using Application.Users.DTOs;
using Domain.Entities;
using Domain.ValueObjects.UserValueObjects;

namespace Application.Users.Mapper;

public static class UserMapper
{
    public static UserListDto ToUserListDto(this User user)=>
        new UserListDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            IsDeleted =  user.IsDeleted,
            Email = user.Email?.Value,
            Mobile = user.Mobile,
            LastLoginAt = user.LastLoginAt,
        };

    public static List<UserListDto> ToUserListDto(this IEnumerable<User> users)=>
        users.Select(x => x.ToUserListDto()).ToList();


    public static UserDetailsDto ToUserDetailsDto(this User user) =>
        new UserDetailsDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email?.Value,
            IsDeleted = user.IsDeleted,
            Mobile = user.Mobile,
            ProfileImageUrl = user.ProfileImageUrl,
            LastLoginAt = user.LastLoginAt,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
}