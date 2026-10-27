using Application.Common.Pagination;
using Application.Interfaces.Repositories.OrganizationInterfaces;
using Application.Organizations.DTOs;
using Application.Organizations.Mapper;
using Domain.Entities;
using MediatR;
using Shared;

namespace Application.Organizations.Queries.GetAllDeleted;

public class GetAllDeletedHandler(IOrganizationReadRepository repository)
    :IRequestHandler<GetAllDeletedQuery,Result<PageResult<OrganizationDto>>>
{
    public async Task<Result<PageResult<OrganizationDto>>> Handle(GetAllDeletedQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllDeletedAsync(
            request.CategoryId,
            request.Page,
            cancellationToken);

        var dto = new PageResult<OrganizationDto>
        {
            Items = result.Items
                .Select(i => i.ToDto())
                .ToList(),

            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };

        return Result<PageResult<OrganizationDto>>.Success(dto);
    }
}