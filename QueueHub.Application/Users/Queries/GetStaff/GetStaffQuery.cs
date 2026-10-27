using Application.Users.DTOs;
using MediatR;
using Shared;

namespace Application.Users.Queries.GetStaff;

public class GetStaffQuery(Guid? branchId):IRequest<Result<List<UserListDto>>>
{
    public Guid? BranchId { get; set; } = branchId;
}