using Application.Interfaces.Repositories.OrganizationInterfaces;
using Domain.Entities;
using MediatR;
using Shared;

namespace Application.Organizations.Queries.GetDeleted;

public class GetDeletedOrganHandler(
    IOrganizationReadRepository readRepository)
    :IRequestHandler<GetDeletedOrganQuery,Result<Organization>>
{
    public async Task<Result<Organization>> Handle(GetDeletedOrganQuery request, CancellationToken cancellationToken)
    {
        var organization = await readRepository.GetDeletedAsync(
            request.Id,
            cancellationToken);

        if (organization is null)
            return Result<Organization>.Failed(
                Error.NotFound("سازمان مورد نظر یافت نشد"));
        
        return Result<Organization>.Success(organization);                                                      
    }
}