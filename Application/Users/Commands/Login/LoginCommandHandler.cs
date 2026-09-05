using Application.Interfaces;
using Application.Users.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using QueueHub.Application.Common;
using Shared;

namespace QueueHub.Application.Users.Commands.Login;

public class LoginCommandHandler(
    IUserReadRepository userReadRepository,
    IRefreshTokenWriteRepository refreshTokenWriteRepository,
    IPasswordHasher passwordHasher,
    IJwtService jwtService,
    IUnitOfWork unitOfWork,
    IOptions<JwtSetting> jwtOptions) : IRequestHandler<LoginCommand, Result<AuthResult>>
{
    private readonly JwtSetting _jwtSettings = jwtOptions.Value;
    
    public async Task<Result<AuthResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userReadRepository.GetByUserNameAsync(request.UserName, cancellationToken);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result<AuthResult>.Failed("نام کاربری یا رمز  عبور اشتباه است.");

        var accessToken = jwtService.GenerateAccessToken(user);
        var refreshTokenValue = jwtService.GenerateRefreshToken();

        var refreshToken = new RefreshToken(
            refreshTokenValue,
            user.Id,
            DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays));

        await refreshTokenWriteRepository.AddAsync(refreshToken, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var result = new AuthResult()
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue
        };
        return Result<AuthResult>.Success("ورود شما موفقیت آمیز بود.", result);
    }
}