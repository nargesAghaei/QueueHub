using Application.Interfaces;
using Application.Users.Commands.UpdateUserProfile;
using Application.Users.Mapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandHandler(IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ILogger<UpdateUserProfileCommandHandler> logger)
    :IRequestHandler<UpdateUserProfileCommand,Result>
{
    private readonly ICurrentUserService _currentUser=currentUserService;
    private readonly IUserRepository _userRepository= userRepository;
    private readonly IUnitOfWork _unitOfWork= unitOfWork;
    private readonly ILogger<UpdateUserProfileCommandHandler> _logger= logger;
    public async Task<Result> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating user profile...");
        
        var user = await _userRepository.GetByIdAsync(
            _currentUser.Id,
            cancellationToken);

        if (user is null)
        {
            _logger.LogError("Updating user profile failed. user with this Id:{Id} not found.",_currentUser.Id);
            return Result.Failed("کاربر مورد نظر یافت نشد.");
        }

        user.UpdateProfile(
            new FirstName(request.FirstName),
            new Lastname(request.LastName),
            new PhoneNumber(request.PhoneNumber),
            new Email(request.Email),
            request.ProfileImageUrl);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("User profile successfully updated.");
        return Result.Success();
    }
}