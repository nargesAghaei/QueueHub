using Application.Users.DTOs;
using MediatR;
using Shared;

namespace QueueHub.Application.Users.Queries.GetStaff;

public class GetStaffQuery(Guid? branchId):IRequest<Result<List<UserListDto>>>
{
    public Guid? BranchId { get; set; } = branchId;
}