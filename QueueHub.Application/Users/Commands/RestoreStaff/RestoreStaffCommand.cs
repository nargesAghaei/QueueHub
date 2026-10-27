using MediatR;
using Shared;

namespace Application.Users.Commands.RestoreStaff;

public class RestoreStaffCommand(Guid id):IRequest<Result>
{
    public Guid Id { get; set; } = id;
}