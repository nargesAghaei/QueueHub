using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.BranchInterfaces;
using MediatR;
using Shared;

namespace Application.Branchs.Commands.Active;

public class ActiveBranchHandler(
    IBranchWriteRepository branchWriteRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService)
    :IRequestHandler<ActiveBranchCommand,Result>
{
    public async Task<Result> Handle(ActiveBranchCommand request, CancellationToken cancellationToken)
    {
        var branch =await branchWriteRepository.GetBranchByIdAsync(
            currentUserService.OrganizationId,
            request.Id,
            cancellationToken);

        if (branch is null)
            return Result.Failed(Error.NotFound("شعبه مورد نظر یافت نشد"));
        
        branch.Activate(currentUserService.Id);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success($"شعبه با نام {branch.Name} فعال شد");
    }
}