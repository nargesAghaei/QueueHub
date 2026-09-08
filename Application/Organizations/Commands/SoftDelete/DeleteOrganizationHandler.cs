using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces.OrganizationInterfaces;
using MediatR;
using Shared;

namespace Application.Organizations.Commands.SoftDelete;

public class DeleteOrganizationHandler(
    IOrganizationReadRepository organizationReadRepository,
    IOrganizationWriteRepository organizationWriteRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteOrganizationCommand, Result>
{
    public async Task<Result> Handle(
        DeleteOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var organization = await organizationReadRepository
            .GetByIdAsync(
                request.Id,
                cancellationToken);

        if (organization is null || organization.IsDeleted)
            return Result.Failed("سازمان مورد نظر پیدا نشد.");

        organization.SoftDelete(currentUserService.Id);
        organizationWriteRepository.Update(organization);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("سازمان با موفقیت حذف شد");
    }
}