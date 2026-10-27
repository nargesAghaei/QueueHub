using Application.Interfaces;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.OrganizationInterfaces;
using Domain.Entities;
using MediatR;
using Shared;

namespace Application.Organizations.Commands.DeactivateOrganization;

public class DeactivateOrganizationHandler(
    IOrganizationReadRepository organizationReadRepository,
    ICurrentUserService currentUserService,
    IOrganizationWriteRepository organizationWriteRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeactivateOrganizationCommand, Result>
{
    public async Task<Result> Handle(
        DeactivateOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var organization = await organizationReadRepository
            .GetByIdAsync(request.Id, cancellationToken);

        if (organization is null || organization.IsDeleted)
            return Result.Failed(
                Error.NotFound("سازمان مورد نظر پیدا نشد."));

        organization.Deactivate(currentUserService.Id);
        organizationWriteRepository.Update(organization);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}