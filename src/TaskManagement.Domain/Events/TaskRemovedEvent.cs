using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Events;

public class TaskRemovedEvent : DomainEvent
{
    public Guid TaskId { get; }
    public Guid ProjectId { get; }

    public TaskRemovedEvent(Guid taskId, Guid projectId)
    {
        TaskId = taskId;
        ProjectId = projectId;
    }
}
