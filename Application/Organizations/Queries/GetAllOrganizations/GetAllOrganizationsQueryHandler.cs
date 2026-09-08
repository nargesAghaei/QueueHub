using Application.Organizations.DTOs;
using Application.Organizations.Mapper;
using Domain.Entities;
using Domain.Interfaces.OrganizationInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Organizations.Queries.GetAllOrganizations;

public class GetAllOrganizationsQueryHandler(
    IOrganizationReadRepository organizationReadRepository,
    ILogger<GetAllOrganizationsQueryHandler> logger)
    : IRequestHandler<
        GetAllOrganizationsQuery,
        Result<List<OrganizationDto>>>
{
    public async Task<Result<List<OrganizationDto>>> Handle(
        GetAllOrganizationsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Getting organizations. CategoryId: {CategoryId}, ServiceId: {ServiceId}",
            request.CategoryId,
            request.ServiceId);

        var organizations =
            await organizationReadRepository.GetAllOrganizationsAsync(
                request.CategoryId,
                request.ServiceId,
                cancellationToken);

        var organizationDtos = organizations
            .Select(x => x.ToDto())
            .ToList();

        return Result<List<OrganizationDto>>.Success("", organizationDtos);
    }
}