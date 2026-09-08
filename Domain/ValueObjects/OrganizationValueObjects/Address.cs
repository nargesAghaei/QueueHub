using Domain.Exceptions;

namespace Domain.ValueObjects.OrganizationValueObjects;

public record Address(string value):BaseRecordValueObject(value)
{
    protected override void Validate()
    {
        if(string.IsNullOrWhiteSpace(value))
            throw new DomainException("Address cannot be null or whitespace.");

        if (value.Length<10)
            throw new DomainException("Address cannot be less than 10 characters.");
        
        if(value.Length>120)
            throw new DomainException("Address cannot be more than 120 characters.");
    }
    
    public static implicit operator string(Address address)
    { 
        return address.value;
    }
}