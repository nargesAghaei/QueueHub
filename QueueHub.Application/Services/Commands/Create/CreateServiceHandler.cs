using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.ServiceInterfaces;
using Application.Services.DTOs;
using Application.Services.Mapper;
using Domain.Entities;
using Domain.ValueObjects.OrganizationValueObjects;
using MediatR;
using Shared;

namespace Application.Services.Commands.Create;

public class CreateServiceHandler(
    IServiceWriteRepository serviceWriteRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService)
    :IRequestHandler<CreateServiceCommand,Result<ServiceDto>>
{
    public async Task<Result<ServiceDto>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var duplicate=await serviceWriteRepository
            .ExistByNameAsync(
                currentUserService.OrganizationId,
                request.Name,
                cancellationToken);

        if (duplicate)
            return Result<ServiceDto>.Failed(
                Error.Conflict("سرویسی با این نام از قبل وجود دارد"));

        var service = Service.Create(
            new Name(request.Name),
            new Description(request.Description),
            request.EstimatedDurationMinutes,
            currentUserService.OrganizationId,
            currentUserService.Id);

        await serviceWriteRepository.AddAsync(service,cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result<ServiceDto>.Success(service.ToDto());
    }
}