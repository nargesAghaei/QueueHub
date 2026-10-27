namespace Application.Organizations.Validators.Messages;

public static class ManagerMessages
{
    public const string FirstNameRequired = "نام مدیر الزامی است.";
    public const string FirstNameTooLong = "نام مدیر نمی‌تواند بیشتر از 100 کاراکتر باشد.";
    public const string LastNameRequired = "نام خانوادگی مدیر الزامی است.";
    public const string LastNameTooLong = "نام خانوادگی مدیر نمی‌تواند بیشتر از 100 کاراکتر باشد.";
    public const string UserNameRequired = "نام کاربری الزامی است.";
    public const string UserNameTooLong = "نام کاربری نمی‌تواند بیشتر از 100 کاراکتر باشد.";
    public const string PasswordRequired = "رمز عبور الزامی است.";
    public const string PasswordTooShort = "رمز عبور باید حداقل 8 کاراکتر باشد.";
    public const string MobileRequired = "شماره موبایل الزامی است.";
    public const string EmailInvalid = "فرمت ایمیل صحیح نیست.";
}