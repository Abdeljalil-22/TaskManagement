using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Events;

public class TaskUnassignedEvent : DomainEvent
{
    public Guid TaskId { get; }
    public Guid PreviousUserId { get; }

    public TaskUnassignedEvent(Guid taskId, Guid previousUserId)
    {
        TaskId = taskId;
        PreviousUserId = previousUserId;
    }
}
