using MediatR;
using Shared;

namespace Application.Branchs.Commands.Delete;

public class DeleteBranchCommand(Guid id):IRequest<Result>
{
    public Guid Id { get; set; } = id;
}