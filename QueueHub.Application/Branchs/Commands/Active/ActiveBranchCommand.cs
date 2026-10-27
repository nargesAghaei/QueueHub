using MediatR;
using Shared;

namespace Application.Branchs.Commands.Active;

public class ActiveBranchCommand(Guid id):IRequest<Result>
{
    public Guid Id { get; set; } = id;
}