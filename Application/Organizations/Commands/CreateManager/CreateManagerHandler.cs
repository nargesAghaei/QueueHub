using Application.Interfaces;
using Application.Users.DTOs;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.OrganizationInterfaces;
using Domain.ValueObjects.OrganizationValueObjects;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QueueHub.Application.Common;
using Shared;

namespace Application.Organizations.Commands.CreateManager;

public class CreateManagerHandler(IUserWriteRepository userWriteRepository,
    IUserReadRepository userReadRepository,
    IPasswordHasher passwordHasher,
    IOptions<JwtSetting> jwtOptions,
    IJwtService jwtService,
    IRefreshTokenWriteRepository refreshTokenWriteRepository,
    IUnitOfWork unitOfWork,
    IRoleRepository roleRepository,
    IOrganizationWriteRepository organizationWriteRepository,
    IOrganizationReadRepository organizationReadRepository,
    ILogger<CreateManagerHandler> logger)
    :IRequestHandler<CreateManagerCommand,Result<AuthResult>>
{
     private readonly JwtSetting _jwtSettings = jwtOptions.Value;

    public async Task<Result<AuthResult>> Handle(
        CreateManagerCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Creating manager with username {UserName}",
            request.UserName);

        var isDuplicate = await userReadRepository.ExistsByUserNameAsync(
            request.UserName,
            cancellationToken);

        if (isDuplicate)
        {
            logger.LogWarning(
                "Manager creation failed. Username {UserName} already exists",
                request.UserName);

            return Result<AuthResult>.Failed("این نام کاربری از قبل وجود دارد.");
        }

        var categoryExists =
            await organizationReadRepository.ExistsByCategoryIdAsync(
                request.OrganizationCategoryId,
                cancellationToken);

        if (!categoryExists)
            return Result<AuthResult>.Failed("دسته‌بندی سازمان پیدا نشد.");

        var passwordHash = passwordHasher.Hash(request.Password);

        var user = User.RegisterUser(
            new FirstName(request.FirstName),
            new Lastname(request.LastName),
            new UserName(request.UserName),
            new PasswordHash(passwordHash),
            new Mobile(request.Mobile),
            new Email(request.EmailManager)
        );

        var role = await roleRepository.GetByNameAsync(
            roleName: RoleNames.Manager,
            cancellationToken);

        if (role is null)
            return Result<AuthResult>.Failed("نقش مدیر پیدا نشد.");

        user.AssignRole(role);

        var organization = Organization.Create(
            new Name(request.OrganizationName),
            request.OrganizationCode,
            new Description(request.OrganizationDescription),
            new PhoneNumber(request.OrganizationPhoneNumber),
            new Email(request.OrganizationEmail),
            user.Id,
            request.OrganizationWebsite,
            request.OrganizationAddress is null
                ? null
                : new Address(request.OrganizationAddress),
            request.OrganizationCity,
            request.OrganizationLogoUrl,
            request.OrganizationCategoryId
        );

        user.SetOrganization(organization.Id);

        await userWriteRepository.AddAsync(user, cancellationToken);
        await organizationWriteRepository.AddAsync(organization, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Manager and organization created successfully. UserId: {UserId}, OrganizationId: {OrganizationId}",
            user.Id,
            organization.Id);

        var accessToken = jwtService.GenerateAccessToken(user);

        var refreshTokenValue = jwtService.GenerateRefreshToken();

        var refreshToken = new RefreshToken(
            refreshTokenValue,
            user.Id,
            DateTime.UtcNow.AddDays(
                _jwtSettings.RefreshTokenExpirationDays));

        await refreshTokenWriteRepository.AddAsync(refreshToken, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var authResult = new AuthResult()
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue
        };

        return Result<AuthResult>.Success("اطلاعات شما با موفقیت ثبت شد", authResult); 
    }
}