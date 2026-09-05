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

public class UpdateUserProfileCommandHandler(IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ILogger<UpdateUserProfileCommandHandler> logger)
    :IRequestHandler<UpdateUserProfileCommand,Result>
{
    public async Task<Result> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating user profile...");
        
        var user = await userWriteRepository.GetByIdAsync(
            currentUserService.Id,
            cancellationToken);

        if (user is null)
        {
            logger.LogError("Updating user profile failed. user with this Id:{Id} not found.",currentUserService.Id);
            return Result.Failed("کاربر مورد نظر یافت نشد.");
        }

        user.UpdateProfile(
            new FirstName(request.FirstName),
            new Lastname(request.LastName),
            new PhoneNumber(request.PhoneNumber),
            new Email(request.Email),
            request.ProfileImageUrl);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("User profile successfully updated.");
        return Result.Success();
    }
}