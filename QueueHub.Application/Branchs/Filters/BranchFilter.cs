namespace Application.Branchs.Filters;

public sealed record BranchFilter(
    Guid? OrganId,
    Guid? ServiceId,
    Guid? CityId,
    bool? IsOpen,
    string? Search,
    bool? IsActive);