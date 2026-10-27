using Application.Users.Commands.UpdateUserName;
using Application.Users.Validators.Messages;
using FluentValidation;

public class UpdateUserNameCommandValidator
    : AbstractValidator<UpdateUserNameCommand>
{
    public UpdateUserNameCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage(UserValidationMessages.UserNameRequired)
            .MinimumLength(4)
            .WithMessage(UserValidationMessages.UserNameMinLength)
            .MaximumLength(30)
            .WithMessage(UserValidationMessages.UserNameMaxLength)
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage(UserValidationMessages.UserNameNotValid);
    }
}