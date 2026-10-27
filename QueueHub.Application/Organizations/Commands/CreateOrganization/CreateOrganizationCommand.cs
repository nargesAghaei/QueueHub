using Application.Auth.DTOs;
using Application.Users.DTOs;
using MediatR;
using Shared;

namespace Application.Organizations.Commands.CreateOrganization;

public class CreateOrganizationCommand:IRequest<Result<AuthResult>>
{
    public string OrganizationName { get; set; } = null!;
    public string OrganizationCode { get; set; } = null!;
    public string OrganizationDescription { get; set; } = null!;
    public string OrganizationPhoneNumber { get; set; } = null!;
    public string OrganizationEmail { get; set; } = null!;
    public string? OrganizationWebsite { get; set; }
    public string? OrganizationAddress { get; set; }
    public Guid? OrganizationCityId { get; set; }
    public Guid OrganizationCategoryId { get; set; } = default!;
    public string? OrganizationLogoUrl { get; set; }
}