using Application.Common.Pagination;
using Domain.Entities;
using MediatR;
using Shared;

namespace Application.Organizations.Queries.GetDeleted;

public class GetDeletedOrganQuery(Guid id):IRequest<Result<Organization>>
{
    public Guid Id { get; set; } = id;
}