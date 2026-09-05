using MediatR;
using Shared;

namespace Application.Users.Commands.DeleteRole;

public class DeleteRoleCommand(int id):IRequest<Result<string>>
{
    public int RoleId { get; set; } = id;
}