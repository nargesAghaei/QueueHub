using Domain.Exceptions;

namespace Domain.ValueObjects.OrganizationValueObjects;

public record PhoneNumber(string value):BaseRecordValueObject(value)
{
    protected override void Validate()
    {
        if(string.IsNullOrWhiteSpace(Value))
            throw new DomainException("phone number cannot be null or empty");
        
        if(value.Length!=11)
            throw new DomainException("phone number must contain 11 digits");
        
        if(!value.StartsWith("0"))
            throw new DomainException("phone number must start with 0");
        
        if (!Value.All(char.IsDigit)) 
            throw new DomainException("phone number must contain only digits");
    }
    
    public static implicit operator string(PhoneNumber phoneNumber)
    { 
        return phoneNumber.value;
    }
}