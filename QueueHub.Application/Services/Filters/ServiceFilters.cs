namespace Application.Services.Filters;

public sealed record ServiceFilters(
    string? Search,
    Guid? OrganId,
    Guid? BranchId,
    bool? IsActive);