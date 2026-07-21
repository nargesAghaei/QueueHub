using MediatR;
using Shared;

namespace Application.Users.Commands.CreateManager;

public class CreateManagerCommand:IRequest<Result<Guid>>
{
    
}