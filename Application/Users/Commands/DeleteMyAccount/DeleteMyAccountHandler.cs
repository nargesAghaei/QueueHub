using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.DeleteMyAccount;

public class DeleteMyAccountHandler(IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteMyAccountHandler> logger,
    ICurrentUserService currentUserService)
    :IRequestHandler<DeleteMyAccountCommand,Result>
{
    private readonly ILogger<DeleteMyAccountHandler> _logger=logger;
    private readonly IUserRepository _userRepository=userRepository;
    private readonly IUnitOfWork _unitOfWork=unitOfWork;
    private readonly ICurrentUserService _currentUserService=currentUserService;
    public async Task<Result> Handle(DeleteMyAccountCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdWithRolesAsync(_currentUserService.Id, cancellationToken);
        if (user is null)
        {
            _logger.LogError("User not found");
            return Result.Failed("حساب شما یافت نشد.");
        }
        _logger.LogInformation("Start deleting user..");
        user.SoftDelete();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Finish deleting user..");
        return Result.Success("حساب شما با موفقیت حذف شد.");
    }
}