using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class JwtService(IConfiguration configuration) : IJwtService
{
    
    private readonly string _secret = configuration["Jwt:SecretKey"]!;

    public string GenerateToken(User user,string activeRole)
    {
        var claims = new[]
        {
            new Claim(
                ClaimTypes.Name,
                user.FirstName.ToString()),
            
            
            new Claim(
                "UserId",
                user.Id.ToString()),


            new Claim(
                "OrganizationId",
                user.OrganizationId.ToString()),


            new Claim(
                ClaimTypes.Role,
                activeRole)
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
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);


        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}