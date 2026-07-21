using Application.Users.Mapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Shared;

namespace Application.Users.Commands.CreateUser;

public class CreateCitizenCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    :IRequestHandler<CreateCitizenCommand,Result<Guid>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<Guid>> Handle(CreateCitizenCommand request, CancellationToken cancellationToken)
    {
        var isDuplicate = await _userRepository.ExistsByUserNameAsync
            (request.UserName
            , cancellationToken);
        if (isDuplicate)
            return Result<Guid>.Failed("این نام کاربری از قبل وجود دارد.");
        
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
        await _userRepository.AddAsync(user, cancellationToken);
        return Result<Guid>.Success("",user.Guid);
    }
}