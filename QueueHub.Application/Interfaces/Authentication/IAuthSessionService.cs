using Application.Auth.DTOs;
using Application.Users.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Authentication;

public interface IAuthSessionService
{
    Task<AuthResult> CreateAsync(
        User user,
        CancellationToken cancellationToken = default);
}