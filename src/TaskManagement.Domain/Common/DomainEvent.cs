using MediatR;

namespace TaskManagement.Domain.Common;

public abstract class DomainEvent : INotification
{
    public DateTime OccurredAt { get; protected set; } = DateTime.UtcNow;
    public Guid EventId { get; protected set; } = Guid.NewGuid();
}
