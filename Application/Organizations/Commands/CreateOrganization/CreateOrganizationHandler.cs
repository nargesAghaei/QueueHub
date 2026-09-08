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

namespace Application.Organizations.Commands.CreateOrganization;

public class CreateOrganizationHandler(
    IUserReadRepository userReadRepository,
    IUserWriteRepository userWriteRepository,
    ICurrentUserService currentUser,
    IRoleRepository roleRepository,
    IOrganizationWriteRepository organizationWriteRepository,
    IOrganizationReadRepository organizationReadRepository,
    IOptions<JwtSetting> jwtOptions,
    IJwtService jwtService,
    IRefreshTokenWriteRepository refreshTokenWriteRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateOrganizationHandler> logger)
    : IRequestHandler<CreateOrganizationCommand, Result<AuthResult>>
{
    private readonly JwtSetting _jwtSettings = jwtOptions.Value;
    
    public async Task<Result<AuthResult>> Handle(
        CreateOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Registering organization for user {Id}",
            currentUser.Id);

        var user = await userReadRepository.GetByIdAsync(currentUser.Id, cancellationToken);

        if (user is null)
        {
            logger.LogWarning(
                "Organization registration failed. User {Id} not found",
                currentUser.Id);

            return Result<AuthResult>.Failed("کاربر پیدا نشد.");
        }

        if (user.OrganizationId.HasValue)
        {
            logger.LogWarning(
                "Organization registration failed. User {Id} already has an organization",
                user.Id);

            return Result<AuthResult>.Failed("این کاربر قبلاً یک سازمان ثبت کرده است.");
        }

        var categoryExists = await organizationReadRepository.ExistsByCategoryIdAsync(
                request.OrganizationCategoryId,
                cancellationToken);

        if (!categoryExists)
            return Result<AuthResult>.Failed("دسته‌بندی سازمان پیدا نشد.");

        var role = await roleRepository.GetByNameAsync(
            roleName: RoleNames.Manager,
            cancellationToken);

        if (role is null)
            return Result<AuthResult>.Failed("نقش مدیر پیدا نشد.");

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

        user.AssignRole(role);
        user.SetOrganization(organization.Id);
        await organizationWriteRepository.AddAsync(organization, cancellationToken);
        userWriteRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Organization registered successfully. UserId: {Id}, OrganizationId: {OrganizationId}",
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

        return Result<AuthResult>.Success("", authResult);
    }
}