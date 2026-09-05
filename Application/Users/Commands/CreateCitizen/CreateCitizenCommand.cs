using Application.Users.DTOs;
using MediatR;
using Shared;

namespace Application.Users.Commands.CreateUser;

public class CreateCitizenCommand:IRequest<Result<AuthResult>>
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Email { get; set; }
}