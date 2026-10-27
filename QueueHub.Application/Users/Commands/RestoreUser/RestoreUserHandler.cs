using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.UserInterfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.RestoreUser;

public class RestoreUserHandler(
    IUserWriteRepository writeRepository,
    IUnitOfWork unitOfWork)
    :IRequestHandler<RestoreUserCommand,Result>
{
    public async Task<Result> Handle(RestoreUserCommand request, CancellationToken cancellationToken)
    {
        var user =await writeRepository
            .GetDeletedUserByIdAsync(
                request.Id,
                cancellationToken);
        
        if (user is null)
            return Result.Failed(
                Error.NotFound("کاربر مورد نظر یافت نشد"));
        
        user.RestoreUser();
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("کاربر مورد نظر با موفقیت بازگردانی شد");
    }
}