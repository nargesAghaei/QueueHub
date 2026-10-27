using Application.Common.Pagination;

namespace QueueHub.Contracts;

public class BranchFilter(
    string? search,
    Guid? organId,
    Guid? serviceId,
    Guid? cityId,
    bool? isClosed
    ):PageRequest;