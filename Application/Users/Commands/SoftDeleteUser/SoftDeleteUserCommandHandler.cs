using Application.Users.Commands.DeleteUser;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.SoftDeleteUser;

public class SoftDeleteUserCommandHandler(IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILogger<SoftDeleteUserCommandHandler> logger)
    :IRequestHandler<SoftDeleteUserCommand,Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<SoftDeleteUserCommandHandler> _logger= logger;
    public async Task<Result> Handle(SoftDeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Guid, cancellationToken);
        _logger.LogInformation("Deleting user with UserName:{UserName}", user.UserName);

        if (user is null)
        {
            _logger.LogError("Deleting user failed. user with this Id:{Id} not found.",user.Id);
            return Result.Failed("User not found");
        }

        user.SoftDelete();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("User with UserName:{UserName} successfully deleted", user.UserName);
        return Result.Success("User successfully deleted");
    }
}