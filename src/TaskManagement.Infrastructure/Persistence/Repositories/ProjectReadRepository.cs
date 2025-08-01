using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.ReadModels;

namespace TaskManagement.Infrastructure.Persistence.Repositories
{
    public class ProjectReadRepository : IProjectReadRepository
    {
        private readonly ReadDbContext _context;

        public ProjectReadRepository(ReadDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectReadModel>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProjectReadModel?> GetByIdAsync(Guid id)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<ProjectReadModel>> GetByOwnerIdAsync(Guid ownerId)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .Where(p => p.OwnerId == ownerId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
