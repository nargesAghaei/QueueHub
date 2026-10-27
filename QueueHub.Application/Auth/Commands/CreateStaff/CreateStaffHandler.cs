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

namespace Application.Auth.Commands.CreateStaff;

public class CreateStaffHandler(IUserWriteRepository userWriteRepository,
    IUserReadRepository userReadRepository,
    IPasswordHasher passwordHasher,
    IAuthSessionService authSessionService,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IRoleRepository roleRepository)
    :IRequestHandler<CreateStaffCommand,Result<AuthResult>>
{
    
    public async Task<Result<AuthResult>> Handle(
        CreateStaffCommand request
        , CancellationToken cancellationToken)
    {
        var isDuplicate = await userReadRepository.ExistsByUserNameAsync
        (request.UserName
            , cancellationToken);
        
        if (isDuplicate)
            return Result<AuthResult>.Failed(
                Error.Conflict("این نام کاربری از قبل وجود دارد."));
        
        
        var passwordHash = passwordHasher.Hash(request.Password);
        var organizationId = currentUserService.OrganizationId;

        if (organizationId==default) 
            return Result<AuthResult>.Failed(
                Error.Forbidden("مدیر فعلی به سازمانی متصل نیست."));
        
        
        var user = User.RegisterStaff(
            new FirstName(request.FirstName),
            new Lastname(request.LastName),
            new UserName(request.UserName),
            new PasswordHash(passwordHash),
            new Mobile(request.PhoneNumber),
            request.BranchId,
            currentUserService.Id,
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
        
        var authResult =await authSessionService.CreateAsync(user, cancellationToken);
        return Result<AuthResult>.Success("",authResult);
    }
}