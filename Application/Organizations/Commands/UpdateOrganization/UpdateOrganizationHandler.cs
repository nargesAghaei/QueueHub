using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces.OrganizationInterfaces;
using Domain.ValueObjects.OrganizationValueObjects;
using Domain.ValueObjects.UserValueObjects;
using MediatR;
using Shared;

namespace Application.Organizations.Commands.UpdateOrganization;

public class UpdateOrganizationCommandHandler(
    IOrganizationReadRepository organizationReadRepository,
    IOrganizationWriteRepository organizationWriteRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateOrganizationCommand, Result>
{
    public async Task<Result> Handle(
        UpdateOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var organization = await organizationReadRepository
            .GetByIdAsync(
                request.OrganizationId,
                cancellationToken);

        if (organization is null || organization.IsDeleted)
            return Result.Failed("سازمان مورد نظر پیدا نشد.");

        organization.Update(
            new Name(request.OrganizationName),
            request.OrganizationCode,
            new Description(request.OrganizationDescription),
            new PhoneNumber(request.OrganizationPhoneNumber),
            new Email(request.OrganizationEmail),
            currentUserService.Id,
            request.OrganizationWebsite,
            request.OrganizationAddress is null
                ? null
                : new Address(request.OrganizationAddress),
            request.OrganizationCity,
            request.OrganizationLogoUrl
        );

        organizationWriteRepository.Update(organization);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("اطلاعات سازمان با موفقیت ویرایش شد.");
    } 
}