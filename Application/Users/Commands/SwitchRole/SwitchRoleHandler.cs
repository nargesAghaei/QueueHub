using Application.Interfaces;
using Application.Users.Commands.SoftDeleteUser;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.SwitchRole;

public class SwitchRoleHandler(IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<SoftDeleteUserCommandHandler> logger,
    ICurrentUserService currentUserService)
    :IRequestHandler<SwitchRoleCommand,Result>
{
    private readonly IUserRepository _userRepository=userRepository;
    private readonly IUnitOfWork _unitOfWork=unitOfWork;
    private readonly ILogger<SoftDeleteUserCommandHandler> _logger = logger;
    private readonly ICurrentUserService _currentUser=currentUserService;
    
    public async Task<Result> Handle(SwitchRoleCommand request, CancellationToken cancellationToken)
    {
        var user=await _userRepository.GetByIdWithRolesAsync(_currentUser.Id,cancellationToken);
        var role=user.UserRoles.Where(r=>r.RoleId==request.RoleId).FirstOrDefault();
        if (role is null)
        {
            _logger.LogError("User does not have this role");
            return Result.Failed("نقش مورد نظر برای این کاربر یافت نشد.");
        }
        
        _logger.LogInformation("Start switching role to {RoleId}..",role.Role.Name);
        user.SwitchRole(request.RoleId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Role switched to {Role}",role.Role.Name);
        return Result.Success();
    }
}