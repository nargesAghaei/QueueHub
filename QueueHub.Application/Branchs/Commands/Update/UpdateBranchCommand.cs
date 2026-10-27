using MediatR;
using Shared;

namespace Application.Branchs.Commands.Update;

public class UpdateBranchCommand:IRequest<Result>
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = null!;
    public Guid CityId { get; set; } = default;
    public string Address { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Description { get; set; }
}