using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.UserInterfaces;
using Domain.Entities;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.UpdateProfile;

public class UpdateProfileCommandHandler(
    IUserWriteRepository userWriteRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdateProfileCommand, Result>
{
    public async Task<Result> Handle(
        UpdateProfileCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userWriteRepository.GetByIdAsync(
            currentUserService.Id,
            cancellationToken);

        if (user is null)
            return Result.Failed(
                Error.NotFound("کاربر مورد نظر یافت نشد."));

        user.UpdateProfile(
            new FirstName(request.FirstName),
            new Lastname(request.LastName),
            new Mobile(request.PhoneNumber),
            new Email(request.Email),
            request.ProfileImageUrl);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}