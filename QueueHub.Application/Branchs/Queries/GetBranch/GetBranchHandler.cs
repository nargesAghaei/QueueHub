using Application.Branchs.DTOs;
using Application.Branchs.Mapper;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories.BranchInterfaces;
using MediatR;
using Shared;

namespace Application.Branchs.Queries.GetBranch;

public class GetBranchHandler(
    IBranchReadRepository branchReadRepository,
    ICurrentUserService currentUserService)
    :IRequestHandler<GetBranchQuery,Result<BranchDto>>
{
    public async Task<Result<BranchDto>> Handle(GetBranchQuery request, CancellationToken cancellationToken)
    {
        var branch = await branchReadRepository.GetBranchByIdAsync(
            currentUserService.OrganizationId,
            request.Id,
            cancellationToken);

        if (branch is null)
            return Result<BranchDto>.Failed(Error.NotFound($"شعبه با نام {branch.Name} پیدا نشد"));

        return Result<BranchDto>.Success(branch.ToDto());
    }
        
}