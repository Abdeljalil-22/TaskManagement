using TaskManagement.Domain.Common;
using TaskManagement.Domain.ValueObjects;
using TaskManagement.Domain.Events;

namespace TaskManagement.Domain.Entities;

public class ProjectTask : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public TaskManagement.Domain.ValueObjects.TaskStatus Status { get; private set; } = TaskManagement.Domain.ValueObjects.TaskStatus.ToDo;
    public Priority Priority { get; private set; } = Priority.Medium;
    public DateTime? DueDate { get; private set; }
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;
    public Guid? AssignedUserId { get; private set; }
    public User? AssignedUser { get; private set; }

    private readonly List<string> _labels = new();
    public IReadOnlyCollection<string> Labels => _labels.AsReadOnly();

    private ProjectTask() { } // EF Constructor

    public ProjectTask(string title, Guid projectId, string? description = null, DateTime? dueDate = null)
    {
        Title = title;
        ProjectId = projectId;
        Description = description;
        DueDate = dueDate;
    }

    public void UpdateDetails(string title, string? description = null, DateTime? dueDate = null)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        SetUpdatedAt();
    }

    public void SetPriority(Priority priority)
    {
        Priority = priority;
        SetUpdatedAt();
    }

    public void ChangeStatus(TaskManagement.Domain.ValueObjects.TaskStatus newStatus)
    {
        if (Status == newStatus) return;

        var oldStatus = Status;
        Status = newStatus;
        SetUpdatedAt();

        AddDomainEvent(new TaskStatusChangedEvent(Id, oldStatus, newStatus));

        if (newStatus == TaskManagement.Domain.ValueObjects.TaskStatus.Done)
        {
            AddDomainEvent(new TaskCompletedEvent(Id, Title));
        }
    }

    public void ChangePriority(Priority newPriority)
    {
        Priority = newPriority;
        SetUpdatedAt();
    }

    public void AssignTo(Guid userId)
    {
        AssignedUserId = userId;
        SetUpdatedAt();
        AddDomainEvent(new TaskAssignedEvent(Id, userId));
    }

    public void Unassign()
    {
        if (AssignedUserId.HasValue)
        {
            var previousUserId = AssignedUserId.Value;
            AssignedUserId = null;
            SetUpdatedAt();
            AddDomainEvent(new TaskUnassignedEvent(Id, previousUserId));
        }
    }

    public void AddLabel(string label)
    {
        if (!_labels.Contains(label))
        {
            _labels.Add(label);
            SetUpdatedAt();
        }
    }

    public void RemoveLabel(string label)
    {
        if (_labels.Remove(label))
        {
            SetUpdatedAt();
        }
    }

    public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.UtcNow && Status != TaskManagement.Domain.ValueObjects.TaskStatus.Done;
}