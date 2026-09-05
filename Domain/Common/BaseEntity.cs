using Domain.Interfaces;

namespace Domain.Entities;

public abstract class BaseEntity<TId> : IEntity<TId>
    where TId : notnull
{
    public TId Id { get; protected set; } = default!;
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public TId? CreatedBy { get; protected set; }
    public TId? UpdatedBy { get; protected set; }
}