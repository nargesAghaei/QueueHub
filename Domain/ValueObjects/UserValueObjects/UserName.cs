using System.Text.RegularExpressions;
using Domain.Exceptions;

namespace Domain.ValueObjects.UserValueObjects;

public record UserName(string Value):BaseRecordValueObject
{
    private static readonly Regex Pattern =
        new(@"^[a-zA-Z0-9_.]+$", RegexOptions.Compiled);
    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
            throw new DomainException("نام کاربری اجباری است.");
        
        if (Value.Length < 4)
            throw new DomainException("نام کاربری باید حداقل 4 کاراکتر باشد");
        
        if (Value.Length > 30)
            throw new DomainException("نام کاربری بیشتر از حدمجاز است.");

        if (!Pattern.IsMatch(Value))
            throw new DomainException("نام کاربری میتواند شامل اعداد حروف زیرخط و (.) باشد.");
    }
    
    public static implicit operator string(UserName userName)
    { 
        return userName.Value;
    }
}