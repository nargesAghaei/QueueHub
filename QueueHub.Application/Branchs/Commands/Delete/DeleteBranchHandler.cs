using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.BranchInterfaces;
using MediatR;
using Shared;

namespace Application.Branchs.Commands.Delete;

public class DeleteBranchHandler(
    IBranchWriteRepository branchWriteRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser)
    :IRequestHandler<DeleteBranchCommand,Result>
{
    public async Task<Result> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        var branch =await branchWriteRepository.GetBranchByIdAsync(
            currentUser.OrganizationId,
            request.Id,
            cancellationToken);

        if (branch is null)
            return Result.Failed(Error.NotFound("شعبه مورد نظر یافت نشد"));
        
        branchWriteRepository.Delete(branch);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success($"شعبه با نام {branch.Name} با موفقیت حذف شد");
    }
}