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
            .WithMessage("نام اجباری است.")
            .MinimumLength(UserConstants.MinNameLength)
            .WithMessage("حداقل طول نام 3 کاراکتر است.")
            .MaximumLength(UserConstants.MaxFirstNameLength)
            .WithMessage("حداکثر طول نام 20 کاراکتر است.");

        
        RuleFor(x => x.LastName)
            .MaximumLength(UserConstants.MaxLastNameLength)
            .WithMessage("حداکثر طول نام خانوادگی 30 کاراکتر است.")
            .MinimumLength(UserConstants.MinLastNameLength)
            .WithMessage("حداقل طول نام خانوادگی 3 کاراکتر است.")
            .NotEmpty()
            .NotNull()
            .WithMessage("نام خانوادگی اجباری است.");

        
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