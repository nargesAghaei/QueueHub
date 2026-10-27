using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.OrganizationInterfaces;
using MediatR;
using Shared;

namespace Application.Organizations.Commands.Delete;

public class DeleteOrganHandler(
    IOrganizationWriteRepository organizationWriteRepository,
    IUnitOfWork unitOfWork)
    :IRequestHandler<DeleteOrganCommand,Result>
{
    public async Task<Result> Handle(DeleteOrganCommand request, CancellationToken cancellationToken)
    {
        var organ = await organizationWriteRepository
            .GetByIdAsync(
                request.Id,
                cancellationToken);

        if (organ is null)
            return Result.Failed(
                Error.NotFound("سازمان مورد نظر یافت نشد"));
        
        organizationWriteRepository.Delete(organ);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}