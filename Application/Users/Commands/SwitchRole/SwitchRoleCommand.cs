using MediatR;
using Shared;

namespace Application.Users.Commands.SwitchRole;

public class SwitchRoleCommand(int id):IRequest<Result>
{
    public int RoleId { get; set; } = id;
}