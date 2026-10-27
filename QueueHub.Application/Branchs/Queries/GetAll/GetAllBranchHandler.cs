using Application.Branchs.DTOs;
using Application.Branchs.Filters;
using Application.Branchs.Mapper;
using Application.Common.Pagination;
using Application.Interfaces.Repositories.BranchInterfaces;
using MediatR;
using Shared;

namespace Application.Branchs.Queries.GetAll;

public class GetAllBranchHandler(IBranchReadRepository branchReadRepository)
    :IRequestHandler<GetAllBranchQuery,Result<PageResult<BranchListDto>>>
{
    public async Task<Result<PageResult<BranchListDto>>> Handle(GetAllBranchQuery request, CancellationToken cancellationToken)
    {
        var filter = new BranchFilter(
            request.OrganId,
            request.ServiceId,
            request.CityId,
            request.IsOpen,
            request.Search,
            request.IsActive);

        var result =await branchReadRepository
            .GetAllBranchesAsync(filter, request.page, cancellationToken);

        var dto = new PageResult<BranchListDto>
        {
            Items = result.Items.Select(i => i.ToListDto()).ToList(),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };
        
        return Result<PageResult<BranchListDto>>.Success(dto);
    }
}