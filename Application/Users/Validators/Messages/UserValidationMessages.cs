namespace Application.Users.Validators.Messages;

public static class UserValidationMessages
{
    public const string FirstNameRequired = "نام اجباری می باشد.";
    public const string FirstNameMinLength = "حداقل طول نام 3 کاراکتر است.";
    public const string FirstNameMaxLength = "حداکثر طول نام 20 کاراکتر است.";
    public const string LastNameRequired = "نام خانوادگی اجباری می باشد.";
    public const string LastNameMaxLength = "حداکثر طول نام خانوادگی 30 کاراکتر است.";
    public const string LastNameMinLength = "حداقل طول نام خانوادگی 3 کاراکتر است.";
    public const string PhoneNumberRequired = "شماره تلفن اجباری می باشد.";
    public const string PhoneNumberInvalid = "شماره تلفن نامعتبر می باشد.";
    public const string UserNameRequired = "نام کاربری اجباری می باشد.";
    public const string UserNameMinLength = "نام کاربری باید حداقل ۴ کاراکتر باشد.";
    public const string UserNameMaxLength = "نام کاربری نمی‌تواند بیشتر از ۳۰ کاراکتر باشد.";
    public const string UserNameNotValid = "نام کاربری فقط می‌تواند شامل حروف، عدد و _ باشد.";
    public const string PasswordRequired = "کلمه عبور اجباری می باشد.";
    public const string RePasswordRequired = "تکرار کلمه عبور اجباری می باشد.";
    public const string PasswordsNotSame = "کلمه عبور و تکرار آن یکسان نیست.";
    public const string EmailNotValid = "ایمیل وارد شده معتبر نیست.";
    public const string PasswordMinLength= "کلمه عبور نمی تواند کمتر از 8 کاراکتر باشد.";
}