using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces.OrganizationInterfaces;
using MediatR;
using Shared;

namespace Application.Organizations.Commands.ActivateOrganization;

public class ActivateOrganizationHandler(
    IOrganizationReadRepository organizationReadRepository,
    IOrganizationWriteRepository organizationWriteRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ActivateOrganizationCommand, Result>
{
    public async Task<Result> Handle(
        ActivateOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var organization = await organizationReadRepository
            .GetByIdAsync(
                request.Id,
                cancellationToken);

        if (organization is null || organization.IsDeleted)
            return Result.Failed("سازمان مورد نظر پیدا نشد.");

        organization.Activate(currentUserService.Id);
        organizationWriteRepository.Update(organization);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("");
    }
}