using Application.Common.Pagination;

namespace Application.Branchs.Filters;

public record DeleteBranchFilter(
    Guid? OrganId,
    Guid? ServiceId,
    string? Search);