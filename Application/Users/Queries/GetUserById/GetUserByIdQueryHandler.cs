using Application.Users.DTOs;
using Application.Users.Mapper;
using Domain.Interfaces;
using MediatR;
using Shared;

namespace Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserByIdQuery, Result<UserDetailsDto>>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<UserDetailsDto>> Handle(GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user == null)
            return Result<UserDetailsDto>.Failed("کاربر یافت نشد.");
        return Result<UserDetailsDto>.Success("", user.ToUserDetailsDto());
    }
}