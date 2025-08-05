using TaskManagement.Domain.Common;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Domain.Events;

public class TaskStatusChangedEvent : DomainEvent
{
    public Guid TaskId { get; }
    public TaskManagement.Domain.ValueObjects.TaskStatus OldStatus { get; }
    public TaskManagement.Domain.ValueObjects.TaskStatus NewStatus { get; }

    public TaskStatusChangedEvent(Guid taskId, TaskManagement.Domain.ValueObjects.TaskStatus oldStatus, TaskManagement.Domain.ValueObjects.TaskStatus newStatus)
    {
        TaskId = taskId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}
