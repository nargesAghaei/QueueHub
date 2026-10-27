using Application.Branchs.DTOs;
using MediatR;
using Shared;

namespace Application.Branchs.Queries.GetBranch;

public class GetBranchQuery(Guid id):IRequest<Result<BranchDto>>
{
    public Guid Id { get; set; } = id;
}