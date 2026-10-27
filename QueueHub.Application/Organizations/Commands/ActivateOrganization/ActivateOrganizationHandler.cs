using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.OrganizationInterfaces;
using Domain.Entities;
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
            return Result.Failed(
                Error.NotFound("سازمان مورد نظر پیدا نشد."));

        organization.Activate(currentUserService.Id);
        organizationWriteRepository.Update(organization);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("");
    }
}