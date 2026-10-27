using Application.Auth.DTOs;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.UserInterfaces;
using Application.Users.DTOs;
using Domain.Constants;
using Domain.Entities;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Shared;

namespace Application.Auth.Commands.CreateCitizen;

public class CreateCitizenCommandHandler(
    IUserWriteRepository userWriteRepository,
    IUserReadRepository userReadRepository,
    IAuthSessionService authSessionService,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    IRoleRepository roleRepository)
    : IRequestHandler<CreateCitizenCommand, Result<AuthResult>>
{
    public async Task<Result<AuthResult>> Handle(CreateCitizenCommand request, CancellationToken cancellationToken)
    {
        var isDuplicate = await userReadRepository.ExistsByUserNameAsync
        (request.UserName
            , cancellationToken);

        if (isDuplicate)
            return Result<AuthResult>.Failed(
                Error.Conflict("این نام کاربری از قبل وجود دارد."));


        var passwordHash = passwordHasher.Hash(request.Password);
        var user = User.RegisterUser(
            new FirstName(request.FirstName),
            new Lastname(request.LastName),
            new UserName(request.UserName),
            new PasswordHash(passwordHash),
            new Mobile(request.PhoneNumber),
            request.Email is null
                ? null
                : new Email(request.Email)
        );

        var Role = await roleRepository
            .GetByNameAsync(
                roleName: RoleNames.Citizen,
                cancellationToken);

        user.AssignRole(Role);
        await userWriteRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var authResult = await authSessionService.CreateAsync(user, cancellationToken);
        return Result<AuthResult>.Success("", authResult);
    }
}