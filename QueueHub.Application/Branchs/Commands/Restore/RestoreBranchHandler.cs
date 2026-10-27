using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.BranchInterfaces;
using MediatR;
using Shared;

namespace Application.Branchs.Commands.Restore;

public class RestoreBranchHandler(
    IBranchWriteRepository branchWriteRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService)
    : IRequestHandler<RestoreBranchCommand, Result>
{
    public async Task<Result> Handle(RestoreBranchCommand request, CancellationToken cancellationToken)
    {
        var branch =await branchWriteRepository.GetDeletedBranchByIdAsync(
            currentUserService.OrganizationId,
            request.Id,
            cancellationToken);

        if (branch is null)
            return Result.Failed(
                Error.NotFound("شعبه موزد نظر یافت نشد"));
        
        branch.Restore(currentUserService.Id);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success($"شعبه با نام {branch.Name} با موفقیت بازگردانی شد");
    }
}