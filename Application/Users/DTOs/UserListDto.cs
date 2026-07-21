namespace Application.Users.DTOs;

public class UserListDto
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string UserName { get; set; }
    public string? Email { get; set; }
    public required string PhoneNumber { get; set; }
    public DateTime? LastLoginAt { get; set; }
}