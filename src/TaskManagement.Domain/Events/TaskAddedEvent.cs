using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Events;

public class TaskAddedEvent : DomainEvent
{
    public Guid TaskId { get; }
    public Guid ProjectId { get; }
    public string TaskTitle { get; }

    public TaskAddedEvent(Guid taskId, Guid projectId, string taskTitle)
    {
        TaskId = taskId;
        ProjectId = projectId;
        TaskTitle = taskTitle;
    }
}
