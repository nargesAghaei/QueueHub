using Domain.Enums;
using Domain.Interfaces;

namespace Domain.Entities;

public class QueueTicket : BaseEntity<Guid>, ISoftDeletable
{
    public int TicketNumber { get; private set; }

    public Guid QueueId { get; private set; }
    public Queue Queue { get; private set; } = null!;

    public Guid CitizenId { get; private set; }
    public User Citizen { get; private set; } = null!;

    public QueueStatus Status { get; private set; }

    public DateTime ReservedAt { get; private set; }

    public DateTime? CalledAt { get; private set; }
    public DateTime? ServedAt { get; private set; }

    public bool IsDeleted { get; private set; }

    private QueueTicket() { }

    public static QueueTicket Create(
        int ticketNumber,
        Guid queueId,
        Guid citizenId,
        Guid createdBy)
    {
        return new QueueTicket
        {
            Id = Guid.NewGuid(),
            TicketNumber = ticketNumber,
            QueueId = queueId,
            CitizenId = citizenId,
            Status = QueueStatus.Waiting,
            ReservedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
            IsDeleted = false
        };
    }

    public void Call(Guid updatedBy)
    {
        Status = QueueStatus.Called;
        CalledAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void StartServing(Guid updatedBy)
    {
        Status = QueueStatus.Serving;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void MarkAsServed(Guid updatedBy)
    {
        Status = QueueStatus.Served;
        ServedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void Cancel(Guid updatedBy)
    {
        Status = QueueStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void MarkAsNoShow(Guid updatedBy)
    {
        Status = QueueStatus.NoShow;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void SoftDelete(Guid updatedBy)
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}