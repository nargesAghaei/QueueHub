using MediatR;
using Shared;

namespace Application.Organizations.Commands.UpdateOrganization;

public class UpdateOrganizationCommand:IRequest<Result>
{
    public Guid OrganizationId { get; set; } = default!;
    public string OrganizationName { get;  set; } = null!;
    public string OrganizationCode { get; private set; } = null!;
    public string OrganizationDescription { get; private set; } = null!;
    public string OrganizationPhoneNumber { get; private set; } = null!;
    public string OrganizationEmail { get; private set; } = null!;
    public string? OrganizationWebsite { get; private set; }
    public string? OrganizationAddress { get; private set; }
    public string? OrganizationCity { get; private set; }
    public Guid OrganizationCategoryId { get; private set; } = default!;
    public string? OrganizationLogoUrl { get; private set; }
}