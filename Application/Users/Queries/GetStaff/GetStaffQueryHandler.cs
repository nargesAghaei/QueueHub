using Application.Interfaces;
using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Interfaces;
using MediatR;
using Shared;

namespace QueueHub.Application.Users.Queries.GetStaff;

public class GetStaffQueryHandler(IUserReadRepository userReadRepository,ICurrentUserService currentUserService)
    :IRequestHandler<GetStaffQuery,Result<List<UserListDto>>>
{
    public async Task<Result<List<UserListDto>>> Handle(GetStaffQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userReadRepository.GetByIdAsync(currentUserService.Id,cancellationToken);
        
        if (user == null)
            return Result<List<UserListDto>>.Failed("آیدی شما در سیستم یافت نشد");

        if (user.OrganizationId.HasValue)
            return Result<List<UserListDto>>.Failed("شما به هیچ ستزمانی تعلق ندارید");
        
        var organizationId=user.OrganizationId.Value;
        var list = await userReadRepository.GetStaffAsync(organizationId,request.BranchId,cancellationToken);
        if (list is null)
            return Result<List<UserListDto>>.Failed("لیست کاربران یافت نشد.");
        return Result<List<UserListDto>>.Success("", list.ToUserListDto());
    }
}