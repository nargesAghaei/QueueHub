using Application.Interfaces;
using Application.Users.Commands.CreateEmployeeCommand;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.CreateStaff;

public class CreateStaffHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IRoleRepository roleRepository,
        ILogger<CreateStaffHandler> logger)
    :IRequestHandler<CreateStaffCommand,Result<Guid>>
{
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly ICurrentUserService _currentUser = currentUserService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CreateStaffHandler> _logger = logger;
    
    public async Task<Result<Guid>> Handle(
        CreateStaffCommand request
        , CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating staff user with username {UserName}", request.UserName);
        var isDuplicate = await _userRepository.ExistsByUserNameAsync
        (request.UserName
            , cancellationToken);
        if (isDuplicate)
        {
            _logger.LogError("Staff creation failed. Username {UserName} already exists",
                request.UserName);
            return Result<Guid>.Failed("این نام کاربری از قبل وجود دارد.");
        }
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        var organizationId = _currentUser.OrganizationId;

        if (organizationId is null)
        {
            _logger.LogError("The current administrator is not connected to an organization.");
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
            createdByManagerId:_currentUser.Id,
            request.Email is null 
                ? null 
                : new Email(request.Email)
        );
        
        var staffRole = await _roleRepository
            .GetByNameAsync(
                roleName:RoleNames.Staff,
                cancellationToken);
        
        user.AssignRole(staffRole);
        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Staff user with username {UserName} created successfully", request.UserName);
        return Result<Guid>.Success("",user.Id); 
    }
}