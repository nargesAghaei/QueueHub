using Application.Auth.DTOs;
using Application.Users.Commands.CreateUser;
using Shared;

namespace Application.Auth;

public interface IAuthService
{
    Task<Result<Guid?>> Login(LoginDto dto, CancellationToken cancellationToken);
    Task<bool> IsValidGuid(Guid guid,CancellationToken cancellationToken);
}