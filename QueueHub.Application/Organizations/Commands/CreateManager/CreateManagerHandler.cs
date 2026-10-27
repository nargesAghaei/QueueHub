using Application.Auth.DTOs;
using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.OrganizationInterfaces;
using Application.Interfaces.Repositories.UserInterfaces;
using Application.Users.Authentication;
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

namespace Application.Organizations.Commands.CreateManager;

public class CreateManagerHandler(IUserWriteRepository userWriteRepository,
    IUserReadRepository userReadRepository,
    IPasswordHasher passwordHasher,
    IAuthSessionService authSessionService,
    IUnitOfWork unitOfWork,
    IRoleRepository roleRepository,
    IOrganizationWriteRepository organizationWriteRepository)
    :IRequestHandler<CreateManagerCommand,Result<AuthResult>>
{
    public async Task<Result<AuthResult>> Handle(
        CreateManagerCommand request,
        CancellationToken cancellationToken)
    {
        var isDuplicate = await userReadRepository.ExistsByUserNameAsync(
            request.UserName,
            cancellationToken);

        if (isDuplicate)
            return Result<AuthResult>.Failed(
                Error.Conflict("این نام کاربری از قبل وجود دارد."));

        var categoryExists =
            await cate.ExistsByCategoryIdAsync(
                request.OrganizationCategoryId,
                cancellationToken);

        if (!categoryExists)
            return Result<AuthResult>.Failed(
                Error.NotFound("دسته‌بندی سازمان پیدا نشد."));

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
            return Result<AuthResult>.Failed(
                Error.NotFound("نقش مدیر پیدا نشد."));

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
            request.OrganizationCityId,
            request.OrganizationLogoUrl,
            request.OrganizationCategoryId
        );

        user.SetOrganization(organization.Id);

        await userWriteRepository.AddAsync(user, cancellationToken);
        await organizationWriteRepository.AddAsync(organization, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var authResult =await authSessionService.CreateAsync(user, cancellationToken);
        return Result<AuthResult>.Success("اطلاعات شما با موفقیت ثبت شد", authResult); 
    }
}