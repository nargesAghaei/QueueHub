using MediatR;
using Shared;

namespace Application.Branchs.Commands.Restore;

public class RestoreBranchCommand(Guid id):IRequest<Result>
{
    public Guid Id { get; set; } = id;
}