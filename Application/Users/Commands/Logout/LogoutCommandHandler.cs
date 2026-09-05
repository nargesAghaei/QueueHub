using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace QueueHub.Application.Users.Commands.Logout;

public class LogoutCommandHandler(
    ICurrentUserService currentUserService,
    IRefreshTokenWriteRepository refreshTokenWriteRepository,
    IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork,
    ILogger<LogoutCommandHandler> logger):IRequestHandler<LogoutCommand,Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await userWriteRepository.GetByIdAsync(
            currentUserService.Id,
            cancellationToken);

        if (user is null)
        {
            logger.LogError(
                "Logout failed. User with Id:{UserId} not found.",
                currentUserService.Id);

            return Result.Failed("کاربر یافت نشد.");
        }

        logger.LogInformation(
            "User with Id:{UserId} is logging out.",
            user.Id);

        await refreshTokenWriteRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "User with Id:{UserId} successfully logged out.",
            user.Id);

        return Result.Success("با موفقیت از حساب کاربری خارج شدید.");
    }
}