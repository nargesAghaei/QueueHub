using Application.Services.DTOs;
using Domain.Entities;

namespace Application.Services.Mapper;

public static class ServiceMapper
{
    public static ServiceDto ToDto(this Service entity) => new ServiceDto(
        entity.Id,
        entity.Name,
        entity.Description,
        entity.EstimatedDurationMinutes);
}