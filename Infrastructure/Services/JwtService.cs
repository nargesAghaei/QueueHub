using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using QueueHub.Application.Common;

namespace Infrastructure.Services;

public class JwtService(IConfiguration configuration,IOptions<JwtSetting> jwtOption) : IJwtService
{
    private readonly JwtSetting _setting=jwtOption.Value;
    private readonly string _secret =
        configuration["Jwt:SecretKey"]
        ?? throw new InvalidOperationException(
            "JWT SecretKey is not configured.");

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.Role, user.ActiveRole.Name),
            new(ClaimTypes.Name, user.FirstName)
        };
        
        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_secret));
        
        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);


        var token =
            new JwtSecurityToken(
                issuer:_setting.Issuer,
                audience:_setting.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_setting.AccessTokenExpirationMinutes),
                signingCredentials: credentials);


        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes=new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principle = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("توکن نامعتبر است");
        }
        return principle;
    }
}