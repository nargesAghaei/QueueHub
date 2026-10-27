using Application.Branchs.Commands.Delete;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.BranchInterfaces;
using MediatR;
using Shared;

namespace Application.Branchs.Commands.SoftDelete;

public class SoftDeleteBranchHandler(
    IBranchWriteRepository branchWriteRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService)
    : IRequestHandler<SoftDeleteBranchCommand, Result>
{
    public async Task<Result> Handle(SoftDeleteBranchCommand request, CancellationToken cancellationToken)
    {
        var branch =await branchWriteRepository.GetBranchByIdAsync(
            currentUserService.OrganizationId,
            request.Id,
            cancellationToken);

        if (branch is null)
            return Result.Failed(
                Error.NotFound("شعبه مورد نظر وجود ندارد"));
        
        branch.SoftDelete(currentUserService.Id);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success($"شعبه با نام {branch.Name} با موفقیت حذف شد");
    }
}