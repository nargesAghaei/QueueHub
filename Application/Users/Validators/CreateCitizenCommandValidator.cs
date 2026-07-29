using Application.Users.Commands.CreateUser;
using Domain.Constants;
using FluentValidation;

namespace Application.Users.Validators;

public class CreateCitizenCommandValidator:AbstractValidator<CreateCitizenCommand>
{
    public CreateCitizenCommandValidator()
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

        
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("نام کاربری الزامی است.")
            .MinimumLength(UserConstants.MinUserNameLength)
            .WithMessage("نام کاربری باید حداقل ۴ کاراکتر باشد.")
            .MaximumLength(UserConstants.MaxUserNameLength)
            .WithMessage("نام کاربری نمی‌تواند بیشتر از ۳۰ کاراکتر باشد.")
            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("نام کاربری فقط می‌تواند شامل حروف، عدد و _ باشد.");
        
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("رمز عبور الزامی است.")
            .MinimumLength(UserConstants.MinPasswordLength)
            .WithMessage("رمز عبور باید حداقل ۸ کاراکتر باشد.");
        
        
        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("ایمیل وارد شده معتبر نیست.");
    }
}