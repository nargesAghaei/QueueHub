using MediatR;
using Shared;

namespace Application.Users.Commands.DeleteStaff;

public class DeleteStaffCommand(Guid id):IRequest<Result>
{
    public Guid Id { get; set; } = id;
}