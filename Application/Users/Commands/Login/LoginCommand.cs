using Application.Users.DTOs;
using MediatR;
using Shared;

namespace QueueHub.Application.Users.Commands.Login;

public class LoginCommand(string userName,string password):IRequest<Result<AuthResult>>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}