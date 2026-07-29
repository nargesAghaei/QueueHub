using Application.Users.Commands.CreateUser;
using Application.Users.Commands.UpdateUserProfile;
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
            .WithMessage("")
            .MinimumLength(UserConstants.MinNameLength)
            .WithMessage("")
            .MaximumLength(UserConstants.MaxFirstNameLength)
            .WithMessage("");

        
        RuleFor(x => x.LastName)
            .MaximumLength(UserConstants.MaxLastNameLength)
            .WithMessage("")
            .MinimumLength(UserConstants.MinLastNameLength)
            .WithMessage("")
            .NotEmpty()
            .NotNull()
            .WithMessage("");

        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("شماره موبایل الزامی است.")
            .Matches(@"^09\d{9}$")
            .WithMessage("شماره موبایل معتبر نیست.");

        
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("ایمیل وارد شده معتبر نیست.");
    }
}