using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.ReadModels;

namespace TaskManagement.Application.Interfaces;

public interface IReadDbContext
{
    DbSet<ProjectReadModel> Projects { get; }
    DbSet<TaskReadModel> Tasks { get; }
}