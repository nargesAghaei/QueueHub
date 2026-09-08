using Application.Users.DTOs;
using MediatR;
using Shared;

namespace QueueHub.Application.Users.Commands.Login;

public class LoginCommand():IRequest<Result<AuthResult>>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}