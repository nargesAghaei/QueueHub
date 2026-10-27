namespace Application.Organizations.Filters;

public record OrganizationFilter(
    Guid? CategoryId,
    Guid? ServiceId);