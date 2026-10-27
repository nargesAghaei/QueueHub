namespace Application.Branchs.DTOs;

public sealed record BranchDto(
    Guid Id,
    string Name,
    Guid CityId,
    string CityName,
    string Address,
    string Description,
    string PhoneNumber,
    string ProvinceName,
    bool IsOpen);