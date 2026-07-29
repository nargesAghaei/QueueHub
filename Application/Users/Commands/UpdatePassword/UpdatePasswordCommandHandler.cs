using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.UpdatePassword;

public class UpdatePasswordCommandHandler(IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ILogger<UpdatePasswordCommandHandler> logger)
    :IRequestHandler<UpdatePasswordCommand,Result>
{
    private readonly ICurrentUserService _currentUser=currentUserService;
    private readonly IUserRepository _userRepository=userRepository;
    private readonly IUnitOfWork _unitOfWork=unitOfWork;
    private readonly ILogger<UpdatePasswordCommandHandler> _logger=logger;
    
    public async Task<Result> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating password");
        if (request.Password != request.RePassword)
        {
            _logger.LogError("Passwords do not match");
            return Result.Failed("پسورد با تکرار آن مشابه نیست.");
        }
        var user =await _userRepository.GetByIdAsync(_currentUser.Id, cancellationToken);
        if (user is null)
        {
            _logger.LogError("Updating user password failed. user with this Id:{UserId} not found.",_currentUser.Id);
            return Result.Failed("کاربر مورد نظر یافت نشد.");
        }
        var hashPass=BCrypt.Net.BCrypt.HashPassword(request.Password);
        user.UpdatePassword(new PasswordHash(hashPass));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("User password with UserName:{UserName} successfully Updated", user.UserName);
        return Result.Success("پسورد آپدیت شد.");
    }
}