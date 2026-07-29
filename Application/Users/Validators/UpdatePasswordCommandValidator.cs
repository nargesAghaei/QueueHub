using Application.Users.Commands.UpdatePassword;
using FluentValidation;

public class UpdatePasswordCommandValidator 
    : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("رمز عبور الزامی است.")
            .MinimumLength(8)
            .WithMessage("رمز عبور باید حداقل ۸ کاراکتر باشد.");


        RuleFor(x => x.RePassword)
            .NotEmpty()
            .WithMessage("تکرار رمز عبور الزامی است.")
            .Equal(x => x.Password)
            .WithMessage("رمز عبور و تکرار آن یکسان نیست.");
    }
}