using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;
using TaskManagement.Application.ReadModels;

namespace TaskManagement.Infrastructure.Persistence
{
    public class ReadDbContext : DbContext
    {
        public ReadDbContext(DbContextOptions<ReadDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProjectReadModel> Projects => Set<ProjectReadModel>();
        public DbSet<TaskReadModel> Tasks => Set<TaskReadModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure read models
            modelBuilder.Entity<ProjectReadModel>(builder =>
            {
                builder.ToView("ProjectsView"); // Map to a database view
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Name).IsRequired();
            });

            modelBuilder.Entity<TaskReadModel>(builder =>
            {
                builder.ToView("TasksView"); // Map to a database view
                builder.HasKey(t => t.Id);
                builder.Property(t => t.Title).IsRequired();
            });
        }
    }
}
