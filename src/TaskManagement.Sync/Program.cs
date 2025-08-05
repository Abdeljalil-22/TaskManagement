using TaskManagement.Infrastructure;
using TaskManagement.Sync.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // Add Infrastructure
        services.AddInfrastructure(
            hostContext.Configuration.GetConnectionString("WriteConnection")!,
            hostContext.Configuration.GetConnectionString("ReadConnection")!
        );

        // Add Sync Service
        services.AddHostedService<DataSyncService>();
    })
    .Build();

await host.RunAsync();
