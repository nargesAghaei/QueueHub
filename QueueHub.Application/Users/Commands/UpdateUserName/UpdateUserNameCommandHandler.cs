using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.RefreshTokenInterfaces;
using Application.Interfaces.Repositories.UserInterfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.UpdateUserName;

public class UpdateUserNameCommandHandler(
    IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork,
    IRefreshTokenWriteRepository refreshTokenWriteRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdateUserNameCommand, Result>
{
    public async Task<Result> Handle(UpdateUserNameCommand request, CancellationToken cancellationToken)
    {
        var user = await userWriteRepository
            .GetByIdAsync(
                currentUserService.Id,
                cancellationToken);

        if (user is null)
            return Result.Failed(
                Error.NotFound("کاربر مورد نظر یافت نشد."));

        var duplicate = await userWriteRepository.ExistByUserNameAsync(
            currentUserService.Id,
            request.UserName,
            cancellationToken);

        if (duplicate)
            return Result.Failed(
                Error.Conflict("این نام کاربری قبلا استفاده شده است"));

        user.UpdateUserName(new UserName(request.UserName));
        await refreshTokenWriteRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("نام کاربری آپدیت شد.");
    }
}