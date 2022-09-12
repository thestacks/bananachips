using BananaChips.Infrastructure.Persistence.Database.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BananaChips.API.Initializers;

public class DatabaseSchemaInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseSchemaInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseSchemaInitializer>>();
        logger.LogInformation("{InitializerName} Starting", nameof(DatabaseSchemaInitializer));
        
        var databaseContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        await using var databaseContext = await databaseContextFactory.CreateDbContextAsync(cancellationToken);
        var migrations = await databaseContext.Database.GetPendingMigrationsAsync(cancellationToken);
        if (!migrations.Any())
        {
            logger.LogInformation("{InitializerName} No migrations to apply, exiting...", nameof(DatabaseSchemaInitializer));
        }
        logger.LogInformation("{InitializerName} Applying migrations...", nameof(DatabaseSchemaInitializer));
        
        await databaseContext.Database.MigrateAsync();
        
        logger.LogInformation("{InitializerName} Migrations have been applied, exiting...", nameof(DatabaseSchemaInitializer));
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}