using Application.Auth.DTOs;
using Application.Users.DTOs;
using MediatR;
using Shared;

namespace Application.Auth.Commands.Login;

public class LoginCommand():IRequest<Result<AuthResult>>
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}