using TaskManagement.Domain.Interfaces;
using Dapper;
using System.Data;

namespace TaskManagement.Application.ReadModels
{
    public interface IProjectReadRepository
    {
        Task<IEnumerable<ProjectReadModel>> GetAllAsync();
        Task<ProjectReadModel> GetByIdAsync(Guid id);
        Task<IEnumerable<ProjectReadModel>> GetByOwnerIdAsync(Guid ownerId);
    }

    public class ProjectReadRepository : IProjectReadRepository
    {
        private readonly IDbConnection _readConnection;

        public ProjectReadRepository(IDbConnection readConnection)
        {
            _readConnection = readConnection;
        }

        public async Task<IEnumerable<ProjectReadModel>> GetAllAsync()
        {
            const string sql = @"
                SELECT p.Id, p.Name, p.Description, p.OwnerId, u.Name as OwnerName,
                       COUNT(t.Id) as TotalTasks,
                       COUNT(CASE WHEN t.Status = 'Completed' THEN 1 END) as CompletedTasks,
                       p.CreatedAt, p.LastUpdated
                FROM Projects p
                LEFT JOIN Users u ON p.OwnerId = u.Id
                LEFT JOIN Tasks t ON p.Id = t.ProjectId
                GROUP BY p.Id, p.Name, p.Description, p.OwnerId, u.Name, p.CreatedAt, p.LastUpdated";

            var projects = await _readConnection.QueryAsync<ProjectReadModel>(sql);
            foreach (var project in projects)
            {
                project.Tasks = (await GetTasksForProject(project.Id)).ToList();
            }
            return projects;
        }

        public async Task<ProjectReadModel> GetByIdAsync(Guid id)
        {
            const string sql = @"
                SELECT p.Id, p.Name, p.Description, p.OwnerId, u.Name as OwnerName,
                       COUNT(t.Id) as TotalTasks,
                       COUNT(CASE WHEN t.Status = 'Completed' THEN 1 END) as CompletedTasks,
                       p.CreatedAt, p.LastUpdated
                FROM Projects p
                LEFT JOIN Users u ON p.OwnerId = u.Id
                LEFT JOIN Tasks t ON p.Id = t.ProjectId
                WHERE p.Id = @Id
                GROUP BY p.Id, p.Name, p.Description, p.OwnerId, u.Name, p.CreatedAt, p.LastUpdated";

            var project = await _readConnection.QuerySingleOrDefaultAsync<ProjectReadModel>(sql, new { Id = id });
            if (project != null)
            {
                project.Tasks = (await GetTasksForProject(id)).ToList();
            }
            return project;
        }

        public async Task<IEnumerable<ProjectReadModel>> GetByOwnerIdAsync(Guid ownerId)
        {
            const string sql = @"
                SELECT p.Id, p.Name, p.Description, p.OwnerId, u.Name as OwnerName,
                       COUNT(t.Id) as TotalTasks,
                       COUNT(CASE WHEN t.Status = 'Completed' THEN 1 END) as CompletedTasks,
                       p.CreatedAt, p.LastUpdated
                FROM Projects p
                LEFT JOIN Users u ON p.OwnerId = u.Id
                LEFT JOIN Tasks t ON p.Id = t.ProjectId
                WHERE p.OwnerId = @OwnerId
                GROUP BY p.Id, p.Name, p.Description, p.OwnerId, u.Name, p.CreatedAt, p.LastUpdated";

            var projects = await _readConnection.QueryAsync<ProjectReadModel>(sql, new { OwnerId = ownerId });
            foreach (var project in projects)
            {
                project.Tasks = (await GetTasksForProject(project.Id)).ToList();
            }
            return projects;
        }

        private async Task<IEnumerable<TaskReadModel>> GetTasksForProject(Guid projectId)
        {
            const string sql = @"
                SELECT t.Id, t.Title, t.Description, t.AssignedUserId, 
                       u.Name as AssignedUserName, t.DueDate, t.Status,
                       t.CreatedAt, t.LastUpdated
                FROM Tasks t
                LEFT JOIN Users u ON t.AssignedUserId = u.Id
                WHERE t.ProjectId = @ProjectId";

            return await _readConnection.QueryAsync<TaskReadModel>(sql, new { ProjectId = projectId });
        }
    }
}
