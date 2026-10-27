using Application.Auth.DTOs;
using Application.Interfaces.Authentication;
using Application.Interfaces.Repositories.UserInterfaces;
using Application.Users.DTOs;
using MediatR;
using Shared;

namespace Application.Auth.Commands.Login;

public class LoginCommandHandler(
    IUserReadRepository userReadRepository,
    IPasswordHasher passwordHasher,
    IAuthSessionService authSessionService) : IRequestHandler<LoginCommand, Result<AuthResult>>
{
    public async Task<Result<AuthResult>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userReadRepository
            .GetByUserNameAsync(
                request.UserName,
                cancellationToken);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result<AuthResult>.Failed(
                Error.Unauthorized("نام کاربری یا رمز  عبور اشتباه است."));

        var authResult = await authSessionService.CreateAsync(
            user,
            cancellationToken
        );

        return Result<AuthResult>.Success(
            "ورود شما موفقیت آمیز بود.", authResult);
    }
}