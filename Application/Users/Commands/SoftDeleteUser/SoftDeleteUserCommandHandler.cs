using Application.Users.Commands.DeleteUser;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.SoftDeleteUser;

public class SoftDeleteUserCommandHandler(IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork,
    IRefreshTokenWriteRepository refreshTokenWriteRepository,
    ILogger<SoftDeleteUserCommandHandler> logger)
    :IRequestHandler<SoftDeleteUserCommand,Result>
{
    public async Task<Result> Handle(SoftDeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userWriteRepository.GetByIdAsync(request.Guid, cancellationToken);

        if (user is null)
        {
            logger.LogError("Deleting user failed. user with this Id:{Id} not found.", request.Guid);
            return Result.Failed("User not found");
        }

        logger.LogInformation("Deleting user with UserName:{UserName}", user.UserName);
        user.SoftDelete();
        await refreshTokenWriteRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("User with UserName:{UserName} successfully deleted", user.UserName);
        return Result.Success("User successfully deleted");
    }
}