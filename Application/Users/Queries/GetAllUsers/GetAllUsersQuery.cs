using Application.Users.DTOs;
using MediatR;
using Shared;

namespace Application.Users.Queries.GetAllUsers;

public class GetAllUsersQuery(bool isDeleted,int? roleId):IRequest<Result<List<UserListDto>>>
{
    public int? RoleId { get; set; } = roleId;
    public bool IsDeleted { get; set; } = isDeleted;
}