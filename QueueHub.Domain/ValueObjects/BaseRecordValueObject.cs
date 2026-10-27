namespace Domain.ValueObjects;

public abstract record BaseRecordValueObject
{
    public string Value { get; }

    protected BaseRecordValueObject(string value)
    {
        Value = value;
        Validate();
    }

    protected abstract void Validate();
}