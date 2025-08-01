using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Infrastructure.Persistence.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<ProjectTask>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .Include(t => t.AssignedUser)
                .Where(t => t.ProjectId == projectId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProjectTask>> GetFilteredAsync(
            Guid? projectId = null,
            Domain.ValueObjects.TaskStatus? status = null,
            Priority? priority = null,
            Guid? assignedUserId = null,
            string? searchTerm = null,
            DateTime? dueDateFrom = null,
            DateTime? dueDateTo = null,
            IEnumerable<string>? labels = null,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .AsQueryable();

            if (projectId.HasValue)
                query = query.Where(t => t.ProjectId == projectId);

            if (status.HasValue)
                query = query.Where(t => t.Status == status);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority);

            if (assignedUserId.HasValue)
                query = query.Where(t => t.AssignedUserId == assignedUserId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(t => t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm));

            if (dueDateFrom.HasValue)
                query = query.Where(t => t.DueDate >= dueDateFrom);

            if (dueDateTo.HasValue)
                query = query.Where(t => t.DueDate <= dueDateTo);

            if (labels != null && labels.Any())
                query = query.Where(t => t.Labels.Any(l => labels.Contains(l)));

            return await query.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(ProjectTask task, CancellationToken cancellationToken = default)
        {
            await _context.Tasks.AddAsync(task, cancellationToken);
        }

        public async Task UpdateAsync(ProjectTask task, CancellationToken cancellationToken = default)
        {
            _context.Tasks.Update(task);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(ProjectTask task, CancellationToken cancellationToken = default)
        {
            _context.Tasks.Remove(task);
            await Task.CompletedTask;
        }
    }
}
