using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.BranchInterfaces;
using Domain.Entities;
using Domain.ValueObjects.OrganizationValueObjects;
using MediatR;
using Shared;

namespace Application.Branchs.Commands.Update;

public class UpdateBranchHandler(
    IBranchWriteRepository branchWriteRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork)
    :IRequestHandler<UpdateBranchCommand,Result>
{
    public async Task<Result> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch =await branchWriteRepository.GetByNameAndIdAsync(
            request.Name,
            request.Id,
            currentUserService.OrganizationId,
            cancellationToken);

        if (branch is null)
            return Result.Failed(
                Error.Conflict("شعبه با این نام از قبل وجود دارد"));

        branch.Update(
            new Name(request.Name),
            request.CityId,
            new Address(request.Address),
            new Description(request.Description),
            new PhoneNumber(request.PhoneNumber),
            currentUserService.Id);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("شعبه با موفقیت آپدیت شد");
    }
}