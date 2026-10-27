using MediatR;
using Shared;

namespace Application.Branchs.Commands.Create;

public record CreateBranchCommand:IRequest<Result>
{
       public string Name { get; set; } = null!;
       public Guid CityId { get; set; } = default;
       public string Address { get; set; } = null!;
       public string PhoneNumber { get; set; } = null!;
       public string? Description { get; set; }
}