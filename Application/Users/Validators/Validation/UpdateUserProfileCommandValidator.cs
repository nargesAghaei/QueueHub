using Application.Users.Commands.CreateUser;
using Application.Users.Commands.UpdateUserProfile;
using Application.Users.Validators.Messages;
using Domain.Constants;
using FluentValidation;

namespace Application.Users.Validators;

public class UpdateUserProfileCommandValidator:AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
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

        
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage(UserValidationMessages.EmailNotValid);
    }
}