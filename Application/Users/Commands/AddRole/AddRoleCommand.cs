using MediatR;
using Shared;

namespace Application.Users.Commands.AddRole;

public class AddRoleCommand(int id):IRequest<Result>
{
    public int RoleId { get; set; } = id;
}