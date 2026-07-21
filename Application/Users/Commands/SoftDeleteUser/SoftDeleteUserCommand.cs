using MediatR;
using Shared;

namespace Application.Users.Commands.DeleteUser;

public class SoftDeleteUserCommand(Guid id):IRequest<Result>
{
    public Guid Guid { get; set; } = id;
}