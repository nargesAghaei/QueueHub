using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Interfaces;
using MediatR;
using Shared;

namespace Application.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(IUserReadRepository userReadRepository)
    : IRequestHandler<GetAllUsersQuery, Result<List<UserListDto>>>
{
    public async Task<Result<List<UserListDto>>> Handle(GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var list = await userReadRepository.GetAllUserAsync(request.IsDeleted,request.RoleId,cancellationToken);
        if (list is null)
            return Result<List<UserListDto>>.Failed("لیست کاربران یافت نشد.");
        return Result<List<UserListDto>>.Success("", list.ToUserListDto());
    }
}