using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Interfaces;
using MediatR;
using Shared;

namespace Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserReadRepository userReadRepository)
    : IRequestHandler<GetUserByIdQuery, Result<UserDetailsDto>>
{
    public async Task<Result<UserDetailsDto>> Handle(GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userReadRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
            return Result<UserDetailsDto>.Failed("کاربر یافت نشد.");
        return Result<UserDetailsDto>.Success("", user.ToUserDetailsDto());
    }
}