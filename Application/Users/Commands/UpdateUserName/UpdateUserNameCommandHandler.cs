using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.UpdateUserName;

public class UpdateUserNameCommandHandler(IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork,
    IRefreshTokenWriteRepository refreshTokenWriteRepository,
    ICurrentUserService currentUserService,
    ILogger<UpdateUserNameCommandHandler> logger)
    :IRequestHandler<UpdateUserNameCommand,Result>
{
    public async Task<Result> Handle(UpdateUserNameCommand request, CancellationToken cancellationToken)
    {       
        logger.LogInformation("Updating UserName..");
        var user =await userWriteRepository.GetByIdAsync(currentUserService.Id, cancellationToken);
        if (user is null)
        {
            logger.LogError("Updating UserName failed. user with this Id:{Id} not found.",currentUserService.Id);
            return Result.Failed("کاربر مورد نظر یافت نشد.");
        }
        user.UpdateUserName(new UserName(request.UserName));
        await refreshTokenWriteRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("UserName successfully updated.");
        return Result.Success("نام کاربری آپدیت شد.");
    }
}