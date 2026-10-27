using System.Security.Claims;
using Application.Interfaces;
using Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? Name
    {
        get
        {
            return httpContextAccessor
                .HttpContext
                .User
                .FindFirst("Name")
                ?.Value.ToString();
        }
    }

    public Guid Id
    {
        get
        {
            var value = httpContextAccessor
                .HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)
                ?.Value;

            return Guid.Parse(value!);
        }
    }


    public Guid OrganizationId
    {
        get
        {
            var value = httpContextAccessor
                .HttpContext?
                .User
                .FindFirst("organizationId")?
                .Value;

            return Guid.TryParse(value, out var id)
                ? id
                : default;
        }
    }


    public string? Role
    {
        get
        {
            return httpContextAccessor
                .HttpContext?
                .User
                .FindFirst(ClaimTypes.Role)
                ?.Value;
        }
    }
}