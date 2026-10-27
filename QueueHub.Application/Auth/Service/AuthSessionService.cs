using Application.Auth.DTOs;
using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.RefreshTokenInterfaces;
using Application.Users.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using QueueHub.Application.Common;

namespace Application.Users.Authentication;

public class AuthSessionService(
    IRefreshTokenWriteRepository refreshTokenWriteRepository,
    IJwtService jwtService,
    IOptions<JwtSetting> options,
    IUnitOfWork unitOfWork):IAuthSessionService
{
    private  readonly JwtSetting _jwtSetting = options.Value;
    
    public async Task<AuthResult> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        var accessToken = jwtService.GenerateAccessToken(user);
        var refreshTokenValue = jwtService.GenerateRefreshToken();

        var refreshToken = new RefreshToken(
            refreshTokenValue,
            user.Id,
            DateTime.UtcNow.AddDays(_jwtSetting.RefreshTokenExpirationDays));

        await refreshTokenWriteRepository.AddAsync(refreshToken, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResult()
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue
        };
    }
}