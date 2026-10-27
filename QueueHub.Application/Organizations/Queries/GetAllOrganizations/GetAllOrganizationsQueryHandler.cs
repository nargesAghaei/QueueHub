using Application.Common.Pagination;
using Application.Interfaces.Repositories.OrganizationInterfaces;
using Application.Organizations.DTOs;
using Application.Organizations.Filters;
using Application.Organizations.Mapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Organizations.Queries.GetAllOrganizations;

public class GetAllOrganizationsQueryHandler(
    IOrganizationReadRepository organizationReadRepository)
    : IRequestHandler<
        GetAllOrganizationsQuery,
        Result<PageResult<OrganizationDto>>>
{
    public async Task<Result<PageResult<OrganizationDto>>> Handle(
        GetAllOrganizationsQuery request,
        CancellationToken cancellationToken)
    {
        var filter=new OrganizationFilter(request.CategoryId, request.ServiceId);
        var result =
            await organizationReadRepository.GetAllOrganizationsAsync(
                filter,
                request.Page,
                cancellationToken);

        var dtoResult = new PageResult<OrganizationDto>
        {
            Items = result.Items
                .Select(o => o.ToDto())
                .ToList(),

            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };

        return Result<PageResult<OrganizationDto>>.Success(dtoResult);
    }
}