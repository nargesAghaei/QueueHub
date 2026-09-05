using Application.Interfaces;
using Application.Users.Commands.CreateCitizen;
using Application.Users.Commands.CreateEmployeeCommand;
using Application.Users.Commands.CreateUser;
using Application.Users.DTOs;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QueueHub.Application.Common;
using Shared;

namespace Application.Users.Commands.CreateStaff;

public class CreateStaffHandler(IUserWriteRepository userWriteRepository,
    IUserReadRepository userReadRepository,
    IPasswordHasher passwordHasher,
    IOptions<JwtSetting> jwtOptions,
    IJwtService jwtService,
    IRefreshTokenWriteRepository refreshTokenWriteRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IRoleRepository roleRepository,
    ILogger<CreateCitizenCommandHandler> logger)
    :IRequestHandler<CreateStaffCommand,Result<AuthResult>>
{
    private readonly JwtSetting _jwtSettings= jwtOptions.Value;
    
    public async Task<Result<AuthResult>> Handle(
        CreateStaffCommand request
        , CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating staff user with username {UserName}", request.UserName);
        var isDuplicate = await userReadRepository.ExistsByUserNameAsync
        (request.UserName
            , cancellationToken);
        if (isDuplicate)
        {
            logger.LogError("Staff creation failed. Username {UserName} already exists",
                request.UserName);
            return Result<AuthResult>.Failed("این نام کاربری از قبل وجود دارد.");
        }
        
        var passwordHash = passwordHasher.Hash(request.Password);
        
        var organizationId = currentUserService.OrganizationId;

        if (organizationId is null)
        {
            logger.LogError("The current administrator is not connected to an organization.");
            return Result<AuthResult>.Failed(
                "مدیر فعلی به سازمانی متصل نیست.");
        }
        
        var user = User.RegisterStaff(
            new FirstName(request.FirstName),
            new Lastname(request.LastName),
            new UserName(request.UserName),
            new PasswordHash(passwordHash),
            new PhoneNumber(request.PhoneNumber),
            organizationId:currentUserService.OrganizationId,
            createdByManagerId:currentUserService.Id,
            request.Email is null 
                ? null 
                : new Email(request.Email)
        );
        
        var Role = await roleRepository
            .GetByNameAsync(
                roleName:RoleNames.Staff,
                cancellationToken);
        
        user.AssignRole(Role);
        await userWriteRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Staff user with username {UserName} created successfully", request.UserName);
        
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