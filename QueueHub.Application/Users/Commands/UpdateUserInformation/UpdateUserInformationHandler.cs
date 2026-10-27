using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.UserInterfaces;
using Application.Users.Commands.UpdateProfile;
using Domain.Entities;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.UpdateUserInformation;

public class UpdateUserInformationHandler(
    IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher
) : IRequestHandler<UpdateUserInformationCommand, Result>
{
    public async Task<Result> Handle(UpdateUserInformationCommand request, CancellationToken cancellationToken)
    {
        var user = await userWriteRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (user is null)
            return Result.Failed(
                Error.NotFound("کاربر مورد نظر یافت نشد."));

        var duplicate=await userWriteRepository
            .ExistByUserNameAsync(
                request.Id,
                request.UserName,
                cancellationToken);

        if (duplicate)
            return Result.Failed(
                Error.Conflict("این نام کاربری قبلا استفاده شده"));
        
        user.UpdateProfile(
            new FirstName(request.FirstName),
            new Lastname(request.LastName),
            new Mobile(request.Mobile),
            new Email(request.Email),
            request.ProfileImageUrl);

        var newPass = passwordHasher.Hash(request.Password);
        user.UpdatePassword(new PasswordHash(newPass));
        user.UpdateUserName(new UserName(request.UserName));

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("اطلاعات کاربر با موفقیت آپدیت شد");
    }
}