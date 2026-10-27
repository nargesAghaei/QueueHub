using Application.Branchs.DTOs;
using Application.Branchs.Mapper;
using Application.Common.Pagination;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories.BranchInterfaces;
using MediatR;
using Shared;

namespace Application.Branchs.Queries.GetAllDeleted;

public class GetAllDeletedHandler(
    IBranchReadRepository branchReadRepository,
    ICurrentUserService currentUserService)
    :IRequestHandler<GetAllDeletedBranchesQuery,Result<PageResult<BranchListDto>>>
{
    public async Task<Result<PageResult<BranchListDto>>> Handle(
        GetAllDeletedBranchesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await branchReadRepository
            .GetAllDeletedBranchesAsync(
                currentUserService.OrganizationId,
                request.Page,
                cancellationToken);

        var pageResult = new PageResult<BranchListDto>
        {
            Items = result.Items
                .Select(branch => branch.ToListDto())
                .ToList(),

            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        };

        return Result<PageResult<BranchListDto>>.Success(pageResult);
    }
}