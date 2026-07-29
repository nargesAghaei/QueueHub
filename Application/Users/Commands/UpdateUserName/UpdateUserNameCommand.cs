using MediatR;
using Shared;

namespace Application.Users.Commands.UpdateUserName;

public class UpdateUserNameCommand:IRequest<Result>
{
    public string UserName { get; set; } = null!;   
}