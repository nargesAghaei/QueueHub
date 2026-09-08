using Application.Users.Commands.CreateUser;
using Application.Users.Validators.Messages;
using Domain.Constants;
using FluentValidation;

namespace Application.Users.Validators.Validation;

public class CreateCitizenCommandValidator:AbstractValidator<CreateCitizenCommand>
{
    public CreateCitizenCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .NotNull()
            .WithMessage(UserValidationMessages.FirstNameRequired)
            .MinimumLength(UserConstants.MinNameLength)
            .WithMessage(UserValidationMessages.FirstNameMinLength)
            .MaximumLength(UserConstants.MaxFirstNameLength)
            .WithMessage(UserValidationMessages.FirstNameMaxLength);

        
        RuleFor(x => x.LastName)
            .MaximumLength(UserConstants.MaxLastNameLength)
            .WithMessage(UserValidationMessages.LastNameMaxLength)
            .MinimumLength(UserConstants.MinLastNameLength)
            .WithMessage(UserValidationMessages.LastNameMinLength)
            .NotEmpty()
            .NotNull()
            .WithMessage(UserValidationMessages.LastNameRequired);

        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage(UserValidationMessages.PhoneNumberRequired)
            .Matches(@"^09\d{9}$")
            .WithMessage(UserValidationMessages.PhoneNumberInvalid);

        
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage(UserValidationMessages.UserNameRequired)
            .MinimumLength(UserConstants.MinUserNameLength)
            .WithMessage(UserValidationMessages.UserNameMinLength)
            .MaximumLength(UserConstants.MaxUserNameLength)
            .WithMessage(UserValidationMessages.UserNameMaxLength)
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage(UserValidationMessages.UserNameNotValid);
        
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(UserValidationMessages.PasswordRequired)
            .MinimumLength(UserConstants.MinPasswordLength)
            .WithMessage(UserValidationMessages.PasswordMinLength);
        
        
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage(UserValidationMessages.EmailNotValid);
    }
}