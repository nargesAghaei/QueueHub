using Application.Users.DTOs;
using MediatR;
using Shared;

namespace Application.Users.Queries.GetDeletedStaff;

public class GetDeletedStaffQuery(Guid id):IRequest<Result<List<UserListDto>>>
{
    public Guid BranchId { get; set; } = id;
}