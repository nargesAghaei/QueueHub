namespace Application.Organizations.DTOs;

public class OrganizationDto
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Website { get;  set; }
    public string? Description { get; set; }
    public Guid? City { get; set; }
    public string? Address { get; set; }
    public string? LogoUrl { get; set; }
}