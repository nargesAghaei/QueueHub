using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories.UserInterfaces;
using Application.Users.DTOs;
using Application.Users.Mapper;
using MediatR;
using Shared;

namespace Application.Users.Queries.GetStaff;

public class GetStaffQueryHandler(
    IUserReadRepository userReadRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetStaffQuery, Result<List<UserListDto>>>
{
    public async Task<Result<List<UserListDto>>> Handle(
        GetStaffQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userReadRepository.GetByIdAsync(
            currentUserService.Id,
            cancellationToken);

        if (user == null)
            return Result<List<UserListDto>>.Failed(
                Error.NotFound("آیدی شما در سیستم یافت نشد"));

        if (user.OrganizationId.HasValue)
            return Result<List<UserListDto>>.Failed(
                Error.NotFound("شما به هیچ سازمانی تعلق ندارید"));

        var organizationId = user.OrganizationId.Value;
        var list = await userReadRepository.GetStaffsAsync(
            organizationId,
            request.BranchId,
            cancellationToken);

        if (list is null)
            return Result<List<UserListDto>>.Failed(
                Error.NotFound("لیست کاربران یافت نشد."));

        return Result<List<UserListDto>>.Success("", list.ToUserListDto());
    }
}