using TaskManagement.Domain.Common;
using TaskManagement.Domain.Events;
using DomainTaskStatus = TaskManagement.Domain.ValueObjects.TaskStatus;
namespace TaskManagement.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public Guid OwnerId { get; private set; }
    public User Owner { get; private set; } = null!;

    private readonly List<ProjectTask> _tasks = new();
    public IReadOnlyCollection<ProjectTask> Tasks => _tasks.AsReadOnly();

    private Project() { } // EF Constructor

    public Project(string name, string description, Guid ownerId)
    {
        Name = name;
        Description = description;
        OwnerId = ownerId;

        AddDomainEvent(new ProjectCreatedEvent(Id, name, ownerId));
    }

    public void UpdateDetails(string name, string description)
    {
        Name = name;
        Description = description;
        SetUpdatedAt();
    }

    public ProjectTask AddTask(string title, string description, DateTime? dueDate = null)
    {
        var task = new ProjectTask(title, Id, description, dueDate);
        _tasks.Add(task);
        
        AddDomainEvent(new TaskAddedEvent(task.Id, Id, title));
        SetUpdatedAt();
        
        return task;
    }

    public void RemoveTask(Guid taskId)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == taskId);
        if (task != null)
        {
            _tasks.Remove(task);
            AddDomainEvent(new TaskRemovedEvent(taskId, Id));
            SetUpdatedAt();
        }
    }
}