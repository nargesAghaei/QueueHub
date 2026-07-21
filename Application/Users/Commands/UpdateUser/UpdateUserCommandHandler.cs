using Application.Users.Mapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Shared;

namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(IUserRepository userRepository)
    :IRequestHandler<UpdateUserCommand,Result>
{
    private readonly IUserRepository _userRepository= userRepository;

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var exists = await _userRepository.ExistsByUserNameAsync(
            request.UserName,
            cancellationToken,
            request.Id);
        
        if (exists)
            throw new Exception("این نام کاربری از قبل وجود دارد.");

        var passwordHash =
            BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        var user = request.ToEntity(passwordHash);
        var result = await _userRepository.UpdateAsync(user, cancellationToken);
        
        if (!result)
            return Result.Failed("شخص مورد نظر یافت نشد.");
        return Result.Success();
    }
}