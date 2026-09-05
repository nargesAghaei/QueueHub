using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.DeleteRole;

public class DeleteRoleHandler(IUserReadRepository userReadRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteRoleHandler>  logger,
    IJwtService jwtService,
    ICurrentUserService currentUserService)
    :IRequestHandler<DeleteRoleCommand,Result<string>>
{
    public async Task<Result<string>> Handle(
        DeleteRoleCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userReadRepository.GetByIdWithRolesAsync(currentUserService.Id, cancellationToken);
        if (user is null)
        {
            logger.LogError("User not found");
            return Result<string>.Failed("حساب شما پیدا نشد.");
        }
        logger.LogInformation("Start Deleting role..");
        user.RemoveRole(request.RoleId);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Role deletion was successful.");
        var accessToken=jwtService.GenerateAccessToken(user);
        return Result<string>.Success("نقش مورد نظر حذف شد.", accessToken);
    }
}