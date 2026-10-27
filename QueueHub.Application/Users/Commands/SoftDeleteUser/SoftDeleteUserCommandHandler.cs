using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.RefreshTokenInterfaces;
using Application.Interfaces.Repositories.UserInterfaces;
using Application.Users.Commands.DeleteUser;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.SoftDeleteUser;

public class SoftDeleteUserCommandHandler(
    IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork,
    IRefreshTokenWriteRepository refreshTokenWriteRepository)
    : IRequestHandler<SoftDeleteUserCommand, Result>
{
    public async Task<Result> Handle(SoftDeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userWriteRepository
            .GetByIdAsync(
                request.Guid,
                cancellationToken);

        if (user is null)
            return Result.Failed(
                Error.NotFound("User not found"));

        user.SoftDelete();
        await refreshTokenWriteRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("User successfully deleted");
    }
}