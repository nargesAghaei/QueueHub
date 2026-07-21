namespace Application.Auth.DTOs;

public class LoginResultDto
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}