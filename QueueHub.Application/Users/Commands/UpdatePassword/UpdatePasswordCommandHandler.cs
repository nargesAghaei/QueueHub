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

namespace Application.Users.Commands.UpdatePassword;

public class UpdatePasswordCommandHandler(
    IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork,
    IRefreshTokenWriteRepository refreshTokenWriteRepository,
    IPasswordHasher passwordHasher,
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdatePasswordCommand, Result>
{
    public async Task<Result> Handle(
        UpdatePasswordCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Password != request.RePassword)
            return Result.Failed(
                Error.Validation("پسورد با تکرار آن مشابه نیست."));

        var user = await userWriteRepository
            .GetByIdAsync(
                currentUserService.Id,
                cancellationToken);

        if (user is null)
            return Result.Failed(
                Error.NotFound("کاربر مورد نظر یافت نشد."));

        var hashPass = passwordHasher.Hash(request.Password);
        user.UpdatePassword(new PasswordHash(hashPass));
        await refreshTokenWriteRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("پسورد آپدیت شد.");
    }
}