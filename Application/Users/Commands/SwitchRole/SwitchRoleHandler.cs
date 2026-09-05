using Application.Interfaces;
using Application.Users.Commands.SoftDeleteUser;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.SwitchRole;

public class SwitchRoleHandler(IUserReadRepository userReadRepository,
    IUnitOfWork unitOfWork,
    IJwtService jwtService,
    ILogger<SoftDeleteUserCommandHandler> logger,
    ICurrentUserService currentUserService)
    :IRequestHandler<SwitchRoleCommand,Result<string>>
{
    public async Task<Result<string>> Handle(SwitchRoleCommand request, CancellationToken cancellationToken)
    {
        var user=await userReadRepository.GetByIdWithRolesAsync(currentUserService.Id,cancellationToken);
        if (user is null)
        {
            logger.LogError("User not found");
            return Result<string>.Failed("کاربر یافت نشد.");
        }
        var role=user.UserRoles.FirstOrDefault(r => r.RoleId==request.RoleId);
        if (role is null)
        {
            logger.LogError("User does not have this role");
            return Result<string>.Failed("نقش مورد نظر برای این کاربر یافت نشد.");
        }
        
        logger.LogInformation("Start switching role to {RoleId}..",role.Role.Name);
        user.SwitchRole(request.RoleId);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var newToken = jwtService.GenerateAccessToken(user);
        logger.LogInformation("Role switched to {Role}",role.Role.Name);
        return Result<string>.Success("نقش شما تغییر کرد",newToken);
    }
}