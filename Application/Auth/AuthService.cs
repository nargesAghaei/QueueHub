using Application.Auth.DTOs;
using Application.Interfaces;
using Application.Users.Commands.CreateUser;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.UserValueObjects;
using Shared;

namespace Application.Auth;

public class AuthService(IUserRepository repo,IJwtService jwtService) : IAuthService
{
    private readonly IJwtService _jwtService = jwtService;
    public async Task<Result<LoginResultDto>> Login(LoginDto dto,CancellationToken cancellationToken)
    {
        var user = await repo.GetByUserNameAsync(dto.UserName,cancellationToken);

        if (user == null)
            return Result<LoginResultDto>.Failed("کاربری با این نام کاربری یافت نشد.");

        var isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

        if (!isValid)
            return Result<LoginResultDto>.Failed("رمز عبور وارد شده نادرست است.");
        
        var token = _jwtService.GenerateToken(user);

        var result = new LoginResultDto
        {
            Token = token,
            UserId = user.Guid
        };
        return Result<LoginResultDto>.Success("",result);
    }

    public async Task<bool> IsValidGuid(Guid guid,CancellationToken cancellationToken)
    {
        return await repo.ExistsById(guid,cancellationToken);
    }
}