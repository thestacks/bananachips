using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BananaChips.Infrastructure.Persistence.Database.Contexts;
using BananaChips.Infrastructure.Services;
using BananaChips.Infrastructure.Services.Meta;
using BananaChips.Infrastructure.Settings;

namespace BananaChips.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddPooledDbContextFactory<DatabaseContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Database"), sqlServerOptions =>
                {
                    sqlServerOptions.CommandTimeout((int)TimeSpan.FromMinutes(3).TotalSeconds);
                });
            })
            .AddScoped(sp =>
                sp.GetRequiredService<IDbContextFactory<DatabaseContext>>().CreateDbContext())
            .AddOptions(configuration)
            .AddServices();
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IAccessTokenProvider, JwtAccessTokenManager>();
    }
 
    private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthenticationSettings>(options => options.LoadFromConfiguration(configuration));
        services.AddOptions();
        return services;
    }
}