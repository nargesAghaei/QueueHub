using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.BranchInterfaces;
using Domain.Entities;
using Domain.ValueObjects.OrganizationValueObjects;
using MediatR;
using Shared;

namespace Application.Branchs.Commands.Create;

public class CreateBranchHandler(
    IBranchWriteRepository branchWriteRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService)
    :IRequestHandler<CreateBranchCommand,Result>
{
    public async Task<Result> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        var exist=await branchWriteRepository.DuplicateNameAsync(
            request.Name,
            currentUserService.OrganizationId
            ,cancellationToken);

        if (exist)
            return Result.Failed(
                Error.Conflict("شعبه بااین نام از قبل وجود دارد"));
        
        var branch = Branch.Create(
            new Name(request.Name),
            request.CityId,
            new Address(request.Address),
            new Description(request.Description),
            new PhoneNumber(request.PhoneNumber),
            currentUserService.OrganizationId,
            currentUserService.Id);

        await branchWriteRepository.AddAsync(branch,cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}