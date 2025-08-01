using MediatR;
using TaskManagement.Application.ReadModels.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure.Persistence;

namespace TaskManagement.Application.Queries.Tasks
{
    public record GetTaskDetailsQuery(Guid TaskId) : IRequest<TaskDetails?>;
    
    public record GetProjectTasksQuery(Guid ProjectId) : IRequest<IReadOnlyList<TaskSummary>>;
    
    public record GetUserTasksQuery(Guid UserId) : IRequest<IReadOnlyList<TaskSummary>>;

    public class TaskQueryHandlers :
        IRequestHandler<GetTaskDetailsQuery, TaskDetails?>,
        IRequestHandler<GetProjectTasksQuery, IReadOnlyList<TaskSummary>>,
        IRequestHandler<GetUserTasksQuery, IReadOnlyList<TaskSummary>>
    {
        private readonly ReadDbContext _readContext;

        public TaskQueryHandlers(ReadDbContext readContext)
        {
            _readContext = readContext;
        }

        public async Task<TaskDetails?> Handle(GetTaskDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _readContext.Tasks
                .AsNoTracking()
                .Where(t => t.Id == request.TaskId)
                .Select(t => new TaskDetails
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description ?? string.Empty,
                    ProjectId = t.ProjectId,
                    ProjectName = t.ProjectName ?? string.Empty,
                    AssignedUserId = t.AssignedUserId,
                    AssignedUserName = t.AssignedUserName,
                    DueDate = t.DueDate,
                    Status = t.Status,
                    Priority = "Medium", // Add proper mapping
                    Labels = new List<string>(), // Add proper mapping
                    CreatedAt = t.CreatedAt,
                    LastUpdated = t.LastUpdated
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<TaskSummary>> Handle(GetProjectTasksQuery request, CancellationToken cancellationToken)
        {
            return await _readContext.Tasks
                .AsNoTracking()
                .Where(t => t.ProjectId == request.ProjectId)
                .Select(t => new TaskSummary
                {
                    Id = t.Id,
                    Title = t.Title,
                    ProjectName = t.ProjectName ?? string.Empty,
                    Status = t.Status,
                    Priority = "Medium", // Add proper mapping
                    DueDate = t.DueDate
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<TaskSummary>> Handle(GetUserTasksQuery request, CancellationToken cancellationToken)
        {
            return await _readContext.Tasks
                .AsNoTracking()
                .Where(t => t.AssignedUserId == request.UserId)
                .Select(t => new TaskSummary
                {
                    Id = t.Id,
                    Title = t.Title,
                    ProjectName = t.ProjectName ?? string.Empty,
                    Status = t.Status,
                    Priority = "Medium", // Add proper mapping
                    DueDate = t.DueDate
                })
                .ToListAsync(cancellationToken);
        }
    }
}
