using Application.Users.Commands.UpdateUserName;
using FluentValidation;

public class UpdateUserNameCommandValidator
    : AbstractValidator<UpdateUserNameCommand>
{
    public UpdateUserNameCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("نام کاربری الزامی است.")

            .MinimumLength(4)
            .WithMessage("نام کاربری باید حداقل ۴ کاراکتر باشد.")

            .MaximumLength(30)
            .WithMessage("نام کاربری نمی‌تواند بیشتر از ۳۰ کاراکتر باشد.")

            .Matches(@"^[a-zA-Z0-9_]+$")
            .WithMessage("نام کاربری فقط می‌تواند شامل حروف انگلیسی، عدد و _ باشد.");
    }
}