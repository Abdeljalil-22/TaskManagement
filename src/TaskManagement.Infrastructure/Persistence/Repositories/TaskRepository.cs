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

        public async Task<ProjectTask?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<ProjectTask>> GetAllAsync()
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectTask>> GetByProjectIdAsync(Guid projectId)
        {
            return await _context.Tasks
                .Include(t => t.AssignedUser)
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectTask>> GetByAssigneeAsync(Guid userId)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.AssignedUserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectTask>> GetByStatusAsync(TaskStatus status)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedUser)
                .Where(t => t.Status == status)
                .ToListAsync();
        }

        public async Task AddAsync(ProjectTask task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public void Update(ProjectTask task)
        {
            _context.Tasks.Update(task);
        }

        public void Delete(ProjectTask task)
        {
            _context.Tasks.Remove(task);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Tasks.AnyAsync(t => t.Id == id);
        }
    }
}
