using Application.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Shared;

namespace Application.Users.Commands.CreateEmployeeCommand;

public class CreateStaffHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IRoleRepository roleRepository)
    :IRequestHandler<CreateStaffCommand,Result<Guid>>
{
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly ICurrentUserService _currentUser = currentUserService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<Result<Guid>> Handle(
        CreateStaffCommand request
        , CancellationToken cancellationToken)
    {
        var isDuplicate = await _userRepository.ExistsByUserNameAsync
        (request.UserName
            , cancellationToken);
        if (isDuplicate)
            return Result<Guid>.Failed("این نام کاربری از قبل وجود دارد.");
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        var organizationId = _currentUser.OrganizationId;

        if (organizationId is null)
        {
            return Result<Guid>.Failed(
                "مدیر فعلی به سازمانی متصل نیست.");
        }
        
        var user = User.RegisterStaff(
            new FirstName(request.FirstName),
            new Lastname(request.LastName),
            new UserName(request.UserName),
            new PasswordHash(passwordHash),
            new PhoneNumber(request.PhoneNumber),
            organizationId:_currentUser.OrganizationId,
            createdByManagerId:_currentUser.UserId,
            request.Email is null 
                ? null 
                : new Email(request.Email)
        );
        
        var staffRole = await _roleRepository
            .GetByNameAsync(
                roleName:RoleNames.Staff,
                cancellationToken);
        
        if (staffRole is null)
        {
            return Result<Guid>.Failed(
                "نقش کارمند در سیستم وجود ندارد.");
        }
        
        user.AssignRole(staffRole);
        await _userRepository.AddAsync(user, cancellationToken);
        return Result<Guid>.Success("",user.Guid);
    }
}