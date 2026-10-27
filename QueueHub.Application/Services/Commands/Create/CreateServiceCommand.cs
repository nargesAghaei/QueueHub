using Application.Services.DTOs;
using MediatR;
using Shared;

namespace Application.Services.Commands.Create;

public record CreateServiceCommand(
    string Name,
    string Description,
    int EstimatedDurationMinutes):IRequest<Result<ServiceDto>>;