using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.UpdatePassword;

public class UpdatePasswordCommandHandler(IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork,
    IRefreshTokenWriteRepository refreshTokenWriteRepository,
    IPasswordHasher passwordHasher,
    ICurrentUserService currentUserService,
    ILogger<UpdatePasswordCommandHandler> logger)
    :IRequestHandler<UpdatePasswordCommand,Result>
{
    public async Task<Result> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating password");
        if (request.Password != request.RePassword)
        {
            logger.LogError("Passwords do not match");
            return Result.Failed("پسورد با تکرار آن مشابه نیست.");
        }
        var user =await userWriteRepository.GetByIdAsync(currentUserService.Id, cancellationToken);
        if (user is null)
        {
            logger.LogError("Updating user password failed. user with this Id:{UserId} not found.",currentUserService.Id);
            return Result.Failed("کاربر مورد نظر یافت نشد.");
        }
        var hashPass=passwordHasher.Hash(request.Password);
        user.UpdatePassword(new PasswordHash(hashPass));
        await refreshTokenWriteRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("User password with UserName:{UserName} successfully Updated", user.UserName);
        return Result.Success("پسورد آپدیت شد.");
    }
}