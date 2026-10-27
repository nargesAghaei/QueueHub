using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.RefreshTokenInterfaces;
using Application.Interfaces.Repositories.UserInterfaces;
using MediatR;
using Shared;

namespace Application.Auth.Commands.Logout;

public class LogoutCommandHandler(
    ICurrentUserService currentUserService,
    IRefreshTokenWriteRepository refreshTokenWriteRepository,
    IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork):IRequestHandler<LogoutCommand,Result>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await userWriteRepository.GetByIdAsync(
            currentUserService.Id,
            cancellationToken);

        if (user is null)
            return Result.Failed(Error.NotFound("کاربر یافت نشد."));

        await refreshTokenWriteRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("با موفقیت از حساب کاربری خارج شدید.");
    }
}