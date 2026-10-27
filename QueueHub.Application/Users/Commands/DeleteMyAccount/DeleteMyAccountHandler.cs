using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.RefreshTokenInterfaces;
using Application.Interfaces.Repositories.UserInterfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.DeleteMyAccount;

public class DeleteMyAccountHandler(IUserReadRepository userReadRepository,
    IUnitOfWork unitOfWork,
    IRefreshTokenWriteRepository refreshTokenWriteRepository,
    ICurrentUserService currentUserService)
    :IRequestHandler<DeleteMyAccountCommand,Result>
{
    public async Task<Result> Handle(DeleteMyAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await userReadRepository.GetByIdWithRolesAsync(currentUserService.Id, cancellationToken);
        if (user is null)
            return Result.Failed(
                Error.NotFound("حساب شما یافت نشد."));
        
        user.SoftDelete();
        await refreshTokenWriteRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("حساب شما با موفقیت حذف شد.");
    }
}