using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories.UserInterfaces;
using Application.Users.DTOs;
using Application.Users.Mapper;
using MediatR;
using Shared;

namespace Application.Users.Queries.GetDeletedStaff;

public class GetDeletedStaffHandler(
    IUserReadRepository userReadRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetDeletedStaffQuery, Result<List<UserListDto>>>
{
    public async Task<Result<List<UserListDto>>> Handle(
        GetDeletedStaffQuery request,
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

        if (user.OrganizationId != currentUserService.OrganizationId)
            return Result<List<UserListDto>>.Failed(
                Error.NotFound("این شعبه متعلق به شما نیست"));

        var staffList = await userReadRepository
            .GetDeletedStaffsAsync(
                request.BranchId,
                cancellationToken);

        if (staffList is null)
            return Result<List<UserListDto>>.Failed(
                Error.NotFound("هیچ کارمند حذف شده ای یافت نشد"));

        return Result<List<UserListDto>>.Success("", staffList.ToUserListDto());
    }
}