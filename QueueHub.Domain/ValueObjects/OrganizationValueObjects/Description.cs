using Domain.Exceptions;

namespace Domain.ValueObjects.OrganizationValueObjects;

public record Description(string value):BaseRecordValueObject(value)
{
    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Value))
            throw new DomainException("Description cannot be empty");
        
        if(value.Length<10)
            throw new DomainException("Description cannot be less than 10 characters");
        
        if(value.Length>254)
            throw new DomainException("Description cannot be more than 254 characters");
    }
    
    public static implicit operator string(Description description)
    { 
        return description.value;
    }
}