using MediatR;
using Shared;

namespace Application.Users.Commands.RestoreUser;

public class RestoreUserCommand(Guid id):IRequest<Result>
{
    public Guid Id { get; set; } = id;
}