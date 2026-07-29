using MediatR;
using Shared;

namespace Application.Users.Commands.AddRole;

public class AddRoleHandler:IRequestHandler<AddRoleCommand,Result>
{
    public Task<Result> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        
    }
}