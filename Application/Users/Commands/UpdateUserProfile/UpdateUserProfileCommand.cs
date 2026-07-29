using MediatR;
using Shared;

namespace Application.Users.Commands.UpdateUserProfile;

public class UpdateUserProfileCommand:IRequest<Result>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Email { get; set; }
    public required string PhoneNumber { get; set; }
    public string? ProfileImageUrl { get; set; }
}