using Application.Users.DTOs;
using MediatR;
using Shared;

namespace Application.Users.Queries.GetAllUsers;

public record GetAllUsersQuery(
    bool IsDeleted,
    int? RoleId)
    :IRequest<Result<List<UserListDto>>>;