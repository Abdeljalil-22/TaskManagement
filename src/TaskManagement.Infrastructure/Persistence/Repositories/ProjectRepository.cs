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

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .Include(p => p.Owner)
                .ToListAsync();
        }

        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        public void Update(Project project)
        {
            _context.Projects.Update(project);
        }

        public void Delete(Project project)
        {
            _context.Projects.Remove(project);
        }

        Task<Project?> IProjectRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Project>> IProjectRepository.GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Project>> IProjectRepository.GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<Project?> IProjectRepository.GetWithTasksAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IProjectRepository.AddAsync(Project project, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IProjectRepository.UpdateAsync(Project project, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task IProjectRepository.DeleteAsync(Project project, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
