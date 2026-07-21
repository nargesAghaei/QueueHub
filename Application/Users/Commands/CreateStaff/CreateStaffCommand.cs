using MediatR;
using Shared;

namespace Application.Users.Commands.CreateEmployeeCommand;

public class CreateStaffCommand:IRequest<Result<Guid>>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Email { get; set; }
}