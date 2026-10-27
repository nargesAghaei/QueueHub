using Domain.Exceptions;

namespace Domain.ValueObjects.UserValueObjects;

public record PasswordHash(string Value) : BaseRecordValueObject(Value)
{
    protected override void Validate()
    {
        if(string.IsNullOrWhiteSpace(Value))
            throw new DomainException("پسورد اجباری است.");
    }
    
    public static implicit operator string(PasswordHash passwordHash)
    { 
        return passwordHash.Value;
    }
}