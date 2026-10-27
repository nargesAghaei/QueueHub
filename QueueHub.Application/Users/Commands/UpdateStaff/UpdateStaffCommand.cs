using MediatR;
using Shared;

namespace Application.Users.Commands.UpdateStaff;

public class UpdateStaffCommand:IRequest<Result>
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Mobile { get; set; }
    public string? Email { get; set; }
    public required string PhoneNumber { get; set; }
    public string? ProfileImageUrl { get; set; }
}