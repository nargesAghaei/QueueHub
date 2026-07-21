using MediatR;
using Shared;

namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommand:IRequest<Result>
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public string? Email { get; set; }
    public required string Password { get; set; }
    public bool IsActive { get; set; }
    public required string PhoneNumber { get; set; }
    public string? ProfileImageUrl { get; set; }
}