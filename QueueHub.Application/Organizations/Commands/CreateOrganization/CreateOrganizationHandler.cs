using Application.Auth.DTOs;
using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.OrganizationInterfaces;
using Application.Interfaces.Repositories.RefreshTokenInterfaces;
using Application.Interfaces.Repositories.UserInterfaces;
using Application.Users.DTOs;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.OrganizationValueObjects;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QueueHub.Application.Common;
using Shared;

namespace Application.Organizations.Commands.CreateOrganization;

public class CreateOrganizationHandler(
    IUserWriteRepository userWriteRepository,
    ICurrentUserService currentUser,
    IRoleRepository roleRepository,
    IOrganizationWriteRepository organizationWriteRepository,
    IAuthSessionService authSessionService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateOrganizationCommand, Result<AuthResult>>
{
    public async Task<Result<AuthResult>> Handle(
        CreateOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userWriteRepository.GetByIdAsync(currentUser.Id, cancellationToken);

        if (user is null)
            return Result<AuthResult>.Failed(
                Error.NotFound("کاربر پیدا نشد."));

        if (user.OrganizationId.HasValue)
            return Result<AuthResult>.Failed(
                Error.Conflict("این کاربر قبلاً یک سازمان ثبت کرده است."));

        var categoryExists = await organizationWriteRepository.ExistsByCategoryIdAsync(
                request.OrganizationCategoryId,
                cancellationToken);

        if (!categoryExists)
            return Result<AuthResult>.Failed(
                Error.NotFound("دسته‌بندی سازمان پیدا نشد."));

        var role = await roleRepository.GetByNameAsync(
            roleName: RoleNames.Manager,
            cancellationToken);

        if (role is null)
            return Result<AuthResult>.Failed(
                Error.NotFound("نقش مدیر پیدا نشد."));

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
            request.OrganizationCityId,
            request.OrganizationLogoUrl,
            request.OrganizationCategoryId
        );

        user.AssignRole(role);
        user.SetOrganization(organization.Id);
        await organizationWriteRepository.AddAsync(organization, cancellationToken);
        userWriteRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var authResult =await authSessionService.CreateAsync(user, cancellationToken);
        return Result<AuthResult>.Success("", authResult);
    }
}