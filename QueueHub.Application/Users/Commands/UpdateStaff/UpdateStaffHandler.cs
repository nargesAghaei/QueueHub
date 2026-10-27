using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.UserInterfaces;
using Application.Users.Commands.UpdateProfile;
using Domain.Entities;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.UpdateStaff;

public class UpdateStaffHandler(
    IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IPasswordHasher passwordHasher
) : IRequestHandler<UpdateStaffCommand, Result>
{
    public async Task<Result> Handle(UpdateStaffCommand request, CancellationToken ct)
    {
        var user = await userWriteRepository
            .GetStaffByIdAsync(
                request.Id,
                currentUserService.OrganizationId,
                ct);

        if (user is null)
            return Result.Failed(
                Error.NotFound("کاربر مورد نظر یافت نشد"));

        var organizationId = currentUserService.OrganizationId;

        if (organizationId == default || user.OrganizationId != organizationId)
            return Result.Failed(
                Error.NotFound("کاربر مورد نظر یافت نشد"));

        user.UpdateProfile(
            new FirstName(request.FirstName),
            new Lastname(request.LastName),
            new Mobile(request.Mobile),
            new Email(request.Email),
            request.ProfileImageUrl);

        user.UpdateUserName(new UserName(request.UserName));

        var newPass = passwordHasher.Hash(request.Password);
        user.UpdatePassword(new PasswordHash(newPass));

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success("اطلاعات کاربر با موفقیت آپدیت شد");
    }
}