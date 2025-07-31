using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Events;

public class TaskAssignedEvent : DomainEvent
{
    public Guid TaskId { get; }
    public Guid UserId { get; }

    public TaskAssignedEvent(Guid taskId, Guid userId)
    {
        TaskId = taskId;
        UserId = userId;
    }
}
