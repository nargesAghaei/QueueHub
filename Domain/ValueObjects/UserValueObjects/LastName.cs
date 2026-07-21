using Domain.Exceptions;

namespace Domain.ValueObjects.UserValueObjects;

public record Lastname(string Value):BaseRecordValueObject
{
    protected override void Validate()
    {
        if(string.IsNullOrWhiteSpace(Value))
            throw new DomainException("نام خانوادگی اجباری است.");
        if (Value.Length < 3)
            throw new DomainException("نام خانوادگی نمیتواند کمتر از 3 کاراکتر باشد.");
        if (Value.Length > 20)
            throw new DomainException("نام خانوادگی نمیتواند بیشتر از 20 کاراکتر باشد.");
    
    }
    public static implicit operator string(Lastname lastName)
    { 
        return lastName.Value;
    }
}