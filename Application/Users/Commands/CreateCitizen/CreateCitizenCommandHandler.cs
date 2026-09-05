using Application.Interfaces;
using Application.Users.Commands.CreateUser;
using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QueueHub.Application.Common;
using Shared;

namespace Application.Users.Commands.CreateCitizen;

public class CreateCitizenCommandHandler(IUserWriteRepository userWriteRepository,
    IUserReadRepository userReadRepository,
    IPasswordHasher passwordHasher,
    IOptions<JwtSetting> jwtOptions,
    IJwtService jwtService,
    IRefreshTokenWriteRepository refreshTokenWriteRepository,
    IUnitOfWork unitOfWork,
    IRoleRepository roleRepository,
    ILogger<CreateCitizenCommandHandler> logger)
    :IRequestHandler<CreateCitizenCommand,Result<AuthResult>>
{
    private readonly JwtSetting _jwtSettings= jwtOptions.Value;
    public async Task<Result<AuthResult>> Handle(CreateCitizenCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating citizen user with username {UserName}", request.UserName);
        
        var isDuplicate = await userReadRepository.ExistsByUserNameAsync
            (request.UserName
            , cancellationToken);
        
        if (isDuplicate)
        {
            logger.LogWarning(
                "Citizen creation failed. Username {UserName} already exists",
                request.UserName);
            return Result<AuthResult>.Failed("این نام کاربری از قبل وجود دارد.");
        }
        
        var passwordHash = passwordHasher.Hash(request.Password);
        var user = User.RegisterCitizen(
            new FirstName(request.FirstName),
            new Lastname(request.LastName),
            new UserName(request.UserName),
            new PasswordHash(passwordHash),
            new PhoneNumber(request.PhoneNumber),
            request.Email is null 
                ? null 
                : new Email(request.Email)
            );
        
        var Role = await roleRepository
            .GetByNameAsync(
                roleName:RoleNames.Citizen,
                cancellationToken);
        
        user.AssignRole(Role);
        await userWriteRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation(
            "Citizen user created successfully. UserId: {UserId}",
            user.Id);
        var accessToken = jwtService.GenerateAccessToken(user);
        var refreshTokenValue = jwtService.GenerateRefreshToken();

        var refreshToken = new RefreshToken(
            refreshTokenValue,
            user.Id,
            DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays));

        await refreshTokenWriteRepository.AddAsync(refreshToken, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var authResult = new AuthResult()
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue
        };
        return Result<AuthResult>.Success("",authResult);
    }
}