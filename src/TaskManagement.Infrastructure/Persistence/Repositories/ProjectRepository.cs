using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Infrastructure.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Where(p => p.OwnerId == ownerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<Project?> GetWithTasksAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task AddAsync(Project project, CancellationToken cancellationToken = default)
        {
            await _context.Projects.AddAsync(project, cancellationToken);
        }

        public async Task UpdateAsync(Project project, CancellationToken cancellationToken = default)
        {
            _context.Projects.Update(project);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Project project, CancellationToken cancellationToken = default)
        {
            _context.Projects.Remove(project);
            await Task.CompletedTask;
        }

        public void Delete(Project project)
        {
            _context.Projects.Remove(project);
        }
    }
}
