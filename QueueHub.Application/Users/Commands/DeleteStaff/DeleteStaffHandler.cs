using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.UserInterfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.DeleteStaff;

public class DeleteStaffHandler(
    ICurrentUserService currentUserService,
    IUserWriteRepository writeRepository,
    IUnitOfWork unitOfWork)
    :IRequestHandler<DeleteStaffCommand,Result>
{
    public async Task<Result> Handle(DeleteStaffCommand request, CancellationToken ct)
    {
        var user =await writeRepository.GetStaffByIdAsync(
            request.Id,
            currentUserService.OrganizationId,
            ct);

        if (user is null)
            return Result.Failed(
                Error.NotFound("کاربر مورد نظر یافت نشد"));
        
        user.SoftDelete();
        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success("کارمند مورد نظر حذف شد");
    }
}