using Domain.Exceptions;

namespace Domain.ValueObjects.UserValueObjects;

public record FirstName(string Value) : BaseRecordValueObject(Value)
{
    protected override void Validate()
    {
        if(string.IsNullOrWhiteSpace(Value))
            throw new DomainException("نام اجباری است.");
        if (Value.Length < 3)
            throw new DomainException("نام نمیتواند کمتر از 3 کاراکتر باشد.");
        if (Value.Length > 20)
            throw new DomainException("نام نمیتواند بیشتر از 20 کاراکتر باشد.");
    
    }
    public static implicit operator string(FirstName name)
    { 
        return name.Value;
    }
}