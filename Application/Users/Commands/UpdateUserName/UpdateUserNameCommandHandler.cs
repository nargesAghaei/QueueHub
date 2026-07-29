using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.UpdateUserName;

public class UpdateUserNameCommandHandler(IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ILogger<UpdateUserNameCommandHandler> logger)
    :IRequestHandler<UpdateUserNameCommand,Result>
{
    private readonly IUserRepository _userRepository=userRepository;
    private readonly ICurrentUserService _currentUser=currentUserService;
    private  readonly IUnitOfWork _unitOfWork=unitOfWork;
    private readonly ILogger<UpdateUserNameCommandHandler> _logger=logger;
    public async Task<Result> Handle(UpdateUserNameCommand request, CancellationToken cancellationToken)
    {       
        _logger.LogInformation("Updating UserName..");
        var user =await _userRepository.GetByIdAsync(_currentUser.Id, cancellationToken);
        if (user is null)
        {
            _logger.LogError("Updating UserName failed. user with this Id:{Id} not found.",_currentUser.Id);
            return Result.Failed("کاربر مورد نظر یافت نشد.");
        }
        user.UpdateUserName(new UserName(request.UserName));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("UserName successfully updated.");
        return Result.Success("نام کاربری آپدیت شد.");
    }
}