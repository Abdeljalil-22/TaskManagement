using TaskManagement.Domain.Entities;

namespace TaskManagement.Domain.Interfaces;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    Task<Project?> GetWithTasksAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Project project, CancellationToken cancellationToken = default);
    Task UpdateAsync(Project project, CancellationToken cancellationToken = default);
    Task DeleteAsync(Project project, CancellationToken cancellationToken = default);
}

