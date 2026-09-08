using Application.Organizations.DTOs;
using Application.Organizations.Mapper;
using Domain.Interfaces.OrganizationInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Organizations.Queries.GetOrganizationById;

public class GetOrganizationByIdQueryHandler(
    IOrganizationReadRepository organizationReadRepository,
    ILogger<GetOrganizationByIdQueryHandler> logger)
    : IRequestHandler<
        GetOrganizationByIdQuery,
        Result<OrganizationDto>>
{
    public async Task<Result<OrganizationDto>> Handle(
        GetOrganizationByIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Getting organization with Id: {OrganizationId}",
            request.Id);

        var organization = await organizationReadRepository
            .GetByIdAsync(
                request.Id,
                cancellationToken);

        if (organization is null || organization.IsDeleted)
        {
            logger.LogWarning(
                "Organization with Id {OrganizationId} was not found",
                request.Id);

            return Result<OrganizationDto>.Failed("سازمان مورد نظر پیدا نشد.");
        }

        var organizationDto = organization.ToDto();
        return Result<OrganizationDto>.Success("", organizationDto);
    }
}