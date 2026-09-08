using Domain.Exceptions;

namespace Domain.ValueObjects.OrganizationValueObjects;

public record Name(string value):BaseRecordValueObject(value)
{
    protected override void Validate()
    {
        if(string.IsNullOrWhiteSpace(value))
            throw new DomainException("Name cannot be empty");
        
        if(value.Length < 3)
            throw new DomainException("Name cannot be less than 3 characters");
        
        if(value.Length>40)
            throw new DomainException("Name cannot be more than 40 characters");
    }
    
    public static implicit operator string(Name name)
    { 
        return name.value;
    }
}