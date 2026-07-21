using Application.Users.DTOs;
using MediatR;
using Shared;

namespace Application.Users.Queries.GetAllUsers;

public class GetAllUsersQuery:IRequest<Result<List<UserListDto>>>
{
    
}