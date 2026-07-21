using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

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

    public Guid UserId
    {
        get
        {
            var value = httpContextAccessor
                .HttpContext?
                .User
                .FindFirst("UserId")
                ?.Value;

            return Guid.Parse(value!);
        }
    }


    public Guid? OrganizationId
    {
        get
        {
            var value = httpContextAccessor
                .HttpContext?
                .User
                .FindFirst("OrganizationId")
                ?.Value;

            return value is null 
                ? null 
                : Guid.Parse(value);
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