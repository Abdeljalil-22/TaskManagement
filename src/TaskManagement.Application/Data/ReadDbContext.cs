using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.ReadModels;

namespace TaskManagement.Application.Data
{
    public class ReadDbContext : DbContext
    {
        public ReadDbContext(DbContextOptions<ReadDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProjectReadModel> Projects { get; set; }
        public DbSet<TaskReadModel> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectReadModel>(entity =>
            {
                entity.ToTable("ProjectReadModels");
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<TaskReadModel>(entity =>
            {
                entity.ToTable("TaskReadModels");
                entity.HasKey(e => e.Id);
                entity.HasOne<ProjectReadModel>()
                    .WithMany()
                    .HasForeignKey(e => e.ProjectId);
            });
        }
    }
}
