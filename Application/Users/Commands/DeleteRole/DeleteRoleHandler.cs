using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.DeleteRole;

public class DeleteRoleHandler(IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteRoleHandler>  logger,
    ICurrentUserService currentUserService)
    :IRequestHandler<DeleteRoleCommand,Result>
{
    private readonly ICurrentUserService _currentUserService=currentUserService;
    private readonly ILogger<DeleteRoleHandler> _logger=logger;
    private readonly IUserRepository _userRepository=userRepository;
    private  readonly IUnitOfWork _unitOfWork=unitOfWork;
    
    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdWithRolesAsync(_currentUserService.Id, cancellationToken);
        if (user is null)
        {
            _logger.LogError("User not found");
            return Result.Failed("حساب شما پیدا نشد.");
        }
        _logger.LogInformation("Start Deleting role..");
        user.RemoveRole(request.RoleId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Role deletion was successful.");
        return Result.Success("نقش مورد نظر حذف شد.");
    }
}