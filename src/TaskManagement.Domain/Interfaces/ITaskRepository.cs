using TaskManagement.Domain.Entities;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Domain.Interfaces;

public interface ITaskRepository
{
    Task<ProjectTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProjectTask>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProjectTask>> GetFilteredAsync(
        Guid? projectId = null,
        TaskManagement.Domain.ValueObjects.TaskStatus? status = null,
        Priority? priority = null,
        Guid? assignedUserId = null,
        string? searchTerm = null,
        DateTime? dueDateFrom = null,
        DateTime? dueDateTo = null,
        IEnumerable<string>? labels = null,
        CancellationToken cancellationToken = default);
    Task AddAsync(ProjectTask task, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProjectTask task, CancellationToken cancellationToken = default);
    Task DeleteAsync(ProjectTask task, CancellationToken cancellationToken = default);
}
