using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Events;

public class TaskCompletedEvent : DomainEvent
{
    public Guid TaskId { get; }
    public string TaskTitle { get; }

    public TaskCompletedEvent(Guid taskId, string taskTitle)
    {
        TaskId = taskId;
        TaskTitle = taskTitle;
    }
}
