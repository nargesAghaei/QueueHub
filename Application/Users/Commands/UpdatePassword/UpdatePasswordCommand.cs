using MediatR;
using Shared;

namespace Application.Users.Commands.UpdatePassword;

public class UpdatePasswordCommand : IRequest<Result>
{
    public string Password { get; set; } = null!;
    public string RePassword { get; set; } = null!;
}