using Application.Users.Commands.CreateUser;
using Application.Users.Mapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Users.Commands.CreateCitizen;

public class CreateCitizenCommandHandler(IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IRoleRepository roleRepository,
    ILogger<CreateCitizenCommandHandler> logger)
    :IRequestHandler<CreateCitizenCommand,Result<Guid>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly ILogger<CreateCitizenCommandHandler> _logger= logger;
    public async Task<Result<Guid>> Handle(CreateCitizenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating citizen user with username {UserName}", request.UserName);
        
        var isDuplicate = await _userRepository.ExistsByUserNameAsync
            (request.UserName
            , cancellationToken);
        if (isDuplicate)
        {
            _logger.LogWarning(
                "Citizen creation failed. Username {UserName} already exists",
                request.UserName);
            return Result<Guid>.Failed("این نام کاربری از قبل وجود دارد.");
        }
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
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
        
        var staffRole = await _roleRepository
            .GetByNameAsync(
                roleName:RoleNames.Citizen,
                cancellationToken);
        
        user.AssignRole(staffRole);
        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.LogInformation(
            "Citizen user created successfully. UserId: {UserId}",
            user.Id);
        return Result<Guid>.Success("",user.Id);
    }
}