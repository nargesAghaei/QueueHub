using Application.Auth.DTOs;
using Application.Users.Commands.CreateUser;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;
using Shared;

namespace Application.Auth;

public class AuthService(IUserRepository repo) : IAuthService
{
    public async Task<Result<Guid?>> Login(LoginDto dto,CancellationToken cancellationToken)
    {
        var user = await repo.GetByUserNameAsync(dto.UserName,cancellationToken);

        if (user == null)
            return Result<Guid?>.Failed("کاربری با این نام کاربری یافت نشد.");

        var isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!isValid)
            return Result<Guid?>.Failed("رمز عبور وارد شده نادرست است.");

        return Result<Guid?>.Success("",user.Guid);
    }

    public async Task<bool> IsValidGuid(Guid guid,CancellationToken cancellationToken)
    {
        return await repo.ExistsById(guid,cancellationToken);
    }
}