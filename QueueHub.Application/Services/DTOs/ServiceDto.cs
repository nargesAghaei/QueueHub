namespace Application.Services.DTOs;

public record ServiceDto(
    Guid Id,
    string Name,
    string Description,
    int EstimatedDurationMinutes);