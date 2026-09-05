using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.DeleteMyAccount;

public class DeleteMyAccountHandler(IUserReadRepository userReadRepository,
    IUnitOfWork unitOfWork,
    IRefreshTokenWriteRepository refreshTokenWriteRepository,
    ILogger<DeleteMyAccountHandler> logger,
    ICurrentUserService currentUserService)
    :IRequestHandler<DeleteMyAccountCommand,Result>
{
    public async Task<Result> Handle(DeleteMyAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await userReadRepository.GetByIdWithRolesAsync(currentUserService.Id, cancellationToken);
        if (user is null)
        {
            logger.LogError("User not found");
            return Result.Failed("حساب شما یافت نشد.");
        }
        logger.LogInformation("Start deleting user..");
        user.SoftDelete();
        await refreshTokenWriteRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Finish deleting user..");
        return Result.Success("حساب شما با موفقیت حذف شد.");
    }
}