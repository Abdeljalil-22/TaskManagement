using MediatR;
using TaskManagement.Application.ReadModels.Projects;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Application.Queries.Projects
{
    public record GetProjectDetailsQuery(Guid ProjectId) : IRequest<ProjectDetails?>;
    
    //public record GetAllProjectsQuery : IRequest<IReadOnlyList<ProjectSummary>>;
    
    public record GetProjectsByOwnerQuery(Guid OwnerId) : IRequest<IReadOnlyList<ProjectSummary>>;

    public class ProjectQueryHandlers : 
        IRequestHandler<GetProjectDetailsQuery, ProjectDetails?>,
        //IRequestHandler<GetAllProjectsQuery, IReadOnlyList<ProjectSummary>>,
        IRequestHandler<GetProjectsByOwnerQuery, IReadOnlyList<ProjectSummary>>
    {
        private readonly IReadDbContext _readContext;

        public ProjectQueryHandlers(IReadDbContext readContext)
        {
            _readContext = readContext;
        }

        public async Task<ProjectDetails?> Handle(GetProjectDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _readContext.Projects
                .AsNoTracking()
                .Where(p => p.Id == request.ProjectId)
                .Select(p => new ProjectDetails
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description ?? string.Empty,
                    OwnerId = p.OwnerId,
                    OwnerName = p.OwnerName ?? string.Empty,
                    TotalTasks = p.TotalTasks,
                    CompletedTasks = p.CompletedTasks,
                    CreatedAt = p.CreatedAt,
                    LastUpdated = p.LastUpdated,
                    Tasks = p.Tasks.Select(t => new TaskDetails
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description ?? string.Empty,
                        AssignedUserId = t.AssignedUserId,
                        AssignedUserName = t.AssignedUserName,
                        DueDate = t.DueDate,
                        Status = t.Status,
                        Priority = "Medium" // Add proper mapping
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        //public async Task<IReadOnlyList<ProjectSummary>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        //{
        //    return await _readContext.Projects
        //        .AsNoTracking()
        //        .Select(p => new ProjectSummary
        //        {
        //            Id = p.Id,
        //            Name = p.Name,
        //            Description = p.Description ?? string.Empty,
        //            TotalTasks = p.TotalTasks,
        //            CompletedTasks = p.CompletedTasks,
        //            LastUpdated = p.LastUpdated ?? p.CreatedAt
        //        })
        //        .ToListAsync(cancellationToken);
        //}

        public async Task<IReadOnlyList<ProjectSummary>> Handle(GetProjectsByOwnerQuery request, CancellationToken cancellationToken)
        {
            return await _readContext.Projects
                .AsNoTracking()
                .Where(p => p.OwnerId == request.OwnerId)
                .Select(p => new ProjectSummary
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description ?? string.Empty,
                    TotalTasks = p.TotalTasks,
                    CompletedTasks = p.CompletedTasks,
                    LastUpdated = p.LastUpdated ?? p.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }
    }
}
