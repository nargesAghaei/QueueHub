using Application.Branchs.DTOs;
using Application.Branchs.Filters;
using Application.Branchs.Mapper;
using Application.Branchs.Queries.GetAll;
using Application.Common.Pagination;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories.BranchInterfaces;
using MediatR;
using Shared;

namespace Application.Branchs.Queries.GetDeleted;

public class GetDeletedHandler(
    IBranchReadRepository branchReadRepository,
    ICurrentUserService currentUserService)
    :IRequestHandler<GetDeletedQuery,Result<BranchDto>>
{
    public async Task<Result<BranchDto>> Handle(
        GetDeletedQuery request,
        CancellationToken cancellationToken)
    {
        var result = await branchReadRepository
            .GetDeletedBranchAsync(
                currentUserService.OrganizationId,
                request.Id,
                cancellationToken);

        if (result is null)
            return Result<BranchDto>.Failed(
                Error.NotFound("شعبه مورد نظر یافت نشد"));

        return Result<BranchDto>.Success(result.ToDto());
    }
}