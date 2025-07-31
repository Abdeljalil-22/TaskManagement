using TaskManagement.Domain.Common;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Domain.Events;

public class TaskStatusChangedEvent : DomainEvent
{
    public Guid TaskId { get; }
    public TaskStatus OldStatus { get; }
    public TaskStatus NewStatus { get; }

    public TaskStatusChangedEvent(Guid taskId, TaskStatus oldStatus, TaskStatus newStatus)
    {
        TaskId = taskId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}
