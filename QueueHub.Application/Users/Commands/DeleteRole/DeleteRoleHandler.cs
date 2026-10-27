using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.UserInterfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.DeleteRole;

public class DeleteRoleHandler(
    IUserReadRepository userReadRepository,
    IUnitOfWork unitOfWork,
    IJwtService jwtService,
    ICurrentUserService currentUserService)
    : IRequestHandler<DeleteRoleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        DeleteRoleCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userReadRepository.GetByIdWithRolesAsync(
            currentUserService.Id,
            cancellationToken);

        if (user is null)
            return Result<string>.Failed(
                Error.NotFound("حساب شما پیدا نشد."));

        user.RemoveRole(request.RoleId);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        var accessToken = jwtService.GenerateAccessToken(user);
        return Result<string>.Success("نقش مورد نظر حذف شد.", accessToken);
    }
}