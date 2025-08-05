using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure.Persistence;
using TaskManagement.Application.ReadModels;

namespace TaskManagement.Sync.Services
{
    public class DataSyncService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DataSyncService> _logger;
        private readonly TimeSpan _syncInterval = TimeSpan.FromSeconds(30);

        public DataSyncService(
            IServiceProvider serviceProvider,
            ILogger<DataSyncService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await SyncData();
                    await Task.Delay(_syncInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while syncing data");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }

        private async Task SyncData()
        {
            using var scope = _serviceProvider.CreateScope();
            var writeContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var readContext = scope.ServiceProvider.GetRequiredService<ReadDbContext>();

            // Sync Projects
            var projects = await writeContext.Projects
                .Include(p => p.Tasks)
                .Include(p => p.Owner)
                .AsNoTracking()
                .ToListAsync();

            foreach (var project in projects)
            {
                var projectReadModel = new ProjectReadModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    Description = project.Description,
                    OwnerId = project.OwnerId,
                    OwnerName = project.Owner?.Name,
                    TotalTasks = project.Tasks.Count,
                    CompletedTasks = project.Tasks.Count(t => t.Status == Domain.ValueObjects.TaskStatus.Done),
                    CreatedAt = project.CreatedAt,
                    LastUpdated = project.LastUpdated,
                    Tasks = project.Tasks.Select(t => new TaskReadModel
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        AssignedUserId = t.AssignedUserId,
                        AssignedUserName = t.AssignedUser?.Name,
                        DueDate = t.DueDate ?? DateTime.MinValue,
                        Status = t.Status.ToString(),
                        CreatedAt = t.CreatedAt,
                        LastUpdated = t.LastUpdated
                    }).ToList()
                };

                var existingProject = await readContext.Projects
                    .Include(p => p.Tasks)
                    .FirstOrDefaultAsync(p => p.Id == project.Id);

                if (existingProject == null)
                {
                    readContext.Projects.Add(projectReadModel);
                }
                else
                {
                    readContext.Entry(existingProject).CurrentValues.SetValues(projectReadModel);
                    
                    // Update tasks
                    foreach (var task in projectReadModel.Tasks)
                    {
                        var existingTask = existingProject.Tasks.FirstOrDefault(t => t.Id == task.Id);
                        if (existingTask == null)
                        {
                            existingProject.Tasks.Add(task);
                        }
                        else
                        {
                            readContext.Entry(existingTask).CurrentValues.SetValues(task);
                        }
                    }

                    // Remove tasks that no longer exist
                    foreach (var existingTask in existingProject.Tasks.ToList())
                    {
                        if (!projectReadModel.Tasks.Any(t => t.Id == existingTask.Id))
                        {
                            existingProject.Tasks.Remove(existingTask);
                        }
                    }
                }
            }

            // Remove projects that no longer exist
            var existingProjectIds = await readContext.Projects.Select(p => p.Id).ToListAsync();
            var currentProjectIds = projects.Select(p => p.Id).ToList();
            var projectsToRemove = existingProjectIds.Except(currentProjectIds);

            foreach (var projectId in projectsToRemove)
            {
                var projectToRemove = await readContext.Projects.FindAsync(projectId);
                if (projectToRemove != null)
                {
                    readContext.Projects.Remove(projectToRemove);
                }
            }

            await readContext.SaveChangesAsync();
            _logger.LogInformation("Data sync completed at: {time}", DateTimeOffset.Now);
        }
    }
}
