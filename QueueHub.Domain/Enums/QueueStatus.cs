namespace Domain.Enums;

public enum QueueStatus
{
    Waiting,
    Called,
    Serving,
    Served,
    Cancelled,
    NoShow
}