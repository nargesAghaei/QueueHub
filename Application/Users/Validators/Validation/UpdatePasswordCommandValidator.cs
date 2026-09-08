using Application.Users.Commands.UpdatePassword;
using Application.Users.Validators.Messages;
using FluentValidation;

namespace Application.Users.Validators.Validation;

public class UpdatePasswordCommandValidator 
    : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(UserValidationMessages.PasswordRequired)
            .MinimumLength(8)
            .WithMessage(UserValidationMessages.PasswordMinLength);


        RuleFor(x => x.RePassword)
            .NotEmpty()
            .WithMessage(UserValidationMessages.RePasswordRequired)
            .Equal(x => x.Password)
            .WithMessage(UserValidationMessages.PasswordsNotSame);
    }
}