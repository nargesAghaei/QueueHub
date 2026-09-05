using System.Text.RegularExpressions;
using Domain.Exceptions;

namespace Domain.ValueObjects.UserValueObjects;

public record Email(string Value) : BaseRecordValueObject(Value)
{
    private static readonly Regex Pattern =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
            return;
        
        if (Value.Length > 254)
            throw new DomainException("طول ایمیل بیشتر از حد مجاز است.");

        if (!Pattern.IsMatch(Value))
            throw new DomainException("ایمیل نامعتبر است.");
    }
    
    public static implicit operator string(Email email)
    { 
        return email.Value;
    }
}