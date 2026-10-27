namespace Application.Branchs.DTOs;

public sealed record BranchListDto(
    Guid Id,
    string Name,
    Guid CityId,
    string CityName,
    string Address,
    string ProvinceName,
    bool IsOpen);