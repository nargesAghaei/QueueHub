using Domain.Exceptions;

namespace Domain.ValueObjects.UserValueObjects;

public record PhoneNumber(string Value):BaseRecordValueObject
{
    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
            throw new DomainException("شماره تلفن اجباری است.");
        
        if (Value.Length != 11)
            throw new DomainException("شماره تلفن باید 11 رقم باشد.");

        if (!Value.All(char.IsDigit))
            throw new DomainException("شماره تلفن باید عدد باشد.");

        if (!Value.StartsWith("0"))
            throw new DomainException("شماره تلفن باید از صفر شروع شود.");
    }
    
    public static implicit operator string(PhoneNumber phoneNumber)
    { 
        return phoneNumber.Value;
    }
}