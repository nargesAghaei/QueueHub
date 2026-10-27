using Application.Interfaces.Repositories.OrganizationInterfaces;
using Application.Organizations.DTOs;
using Application.Organizations.Mapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Organizations.Queries.GetOrganizationById;

public class GetOrganizationByIdQueryHandler(
    IOrganizationReadRepository organizationReadRepository)
    : IRequestHandler<
        GetOrganizationByIdQuery,
        Result<OrganizationDto>>
{
    public async Task<Result<OrganizationDto>> Handle(
        GetOrganizationByIdQuery request,
        CancellationToken cancellationToken)
    {
        var organization = await organizationReadRepository
            .GetByIdAsync(
                request.Id,
                cancellationToken);

        if (organization is null || organization.IsDeleted)
            return Result<OrganizationDto>.Failed(
                Error.NotFound("سازمان مورد نظر پیدا نشد."));
        

        var organizationDto = organization.ToDto();
        return Result<OrganizationDto>.Success("", organizationDto);
    }
}