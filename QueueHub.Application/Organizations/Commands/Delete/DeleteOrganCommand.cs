using MediatR;
using Shared;

namespace Application.Organizations.Commands.Delete;

public class DeleteOrganCommand(Guid id):IRequest<Result>
{
    public Guid Id { get; set; } = id;
}