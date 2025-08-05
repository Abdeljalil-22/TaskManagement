using MediatR;
using MediatR.Registration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Persistence;
using TaskManagement.Infrastructure.Persistence.Repositories;

namespace TaskManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string writeConnectionString, string readConnectionString)
    {
        // Write Context (Command Database)
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(writeConnectionString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }));

        // Read Context (Query Database)
        services.AddDbContext<ReadDbContext>(options =>
            options.UseSqlServer(readConnectionString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }));
        services.AddScoped<IReadDbContext>(provider => provider.GetRequiredService<ReadDbContext>());

        // Repositories
        services.AddScoped<IProjectRepository, ProjectRepository>();
        //services.AddScoped<IProjectReadRepository, ProjectReadRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

   

        return services;
    }
}
