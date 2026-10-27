using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.UserInterfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.RestoreStaff;

public class RestoreStaffHandler(
    IUserWriteRepository userWriteRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RestoreStaffCommand, Result>
{
    public async Task<Result> Handle(RestoreStaffCommand request, CancellationToken cancellationToken)
    {
        var staff = await userWriteRepository
            .GetDeletedStaffByIdAsync(
                request.Id,
                currentUserService.OrganizationId,
                cancellationToken);

        if (staff is null)
            return Result.Failed(
                Error.NotFound("کاربر مورد نظر یافت نشد"));

        staff.RestoreUser();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("کارمند مورد نظر با موفقیت بازگردانی شد");
    }
}