using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.UserInterfaces;
using Application.Users.Commands.SoftDeleteUser;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.SwitchRole;

public class SwitchRoleHandler(
    IUserReadRepository userReadRepository,
    IUnitOfWork unitOfWork,
    IJwtService jwtService,
    ICurrentUserService currentUserService)
    : IRequestHandler<SwitchRoleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SwitchRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await userReadRepository
            .GetByIdWithRolesAsync(
                currentUserService.Id,
                cancellationToken);

        if (user is null)
            return Result<string>.Failed(
                Error.NotFound("کاربر یافت نشد."));

        var role = user.UserRoles.FirstOrDefault(r => r.RoleId == request.RoleId);

        if (role is null)
            return Result<string>.Failed(
                Error.NotFound("نقش مورد نظر برای این کاربر یافت نشد."));

        user.SwitchRole(request.RoleId);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var newToken = jwtService.GenerateAccessToken(user);
        return Result<string>.Success("نقش شما تغییر کرد", newToken);
    }
}