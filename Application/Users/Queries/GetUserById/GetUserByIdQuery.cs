using Application.Users.DTOs;
using MediatR;
using Shared;

namespace Application.Users.Queries.GetUserById;

public class GetUserByIdQuery(Guid id):IRequest<Result<UserDetailsDto>>
{
    public Guid Id { get; set; } = id;
}