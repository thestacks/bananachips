using System.Text;
using BananaChips.API.Initializers;
using BananaChips.Domain.Entities;
using BananaChips.Infrastructure.Persistence.Database.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BananaChips.API.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = !environment.IsDevelopment();
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration.GetValue<string>("Authentication:Tokens:SecretKey"))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false
                };
            });
        services.AddAuthorization();
        return services;
    }
    
    public static IServiceCollection AddInitializers(this IServiceCollection services)
    {
        return services
            .AddTransient<IHostedService, DatabaseSchemaInitializer>()
            .AddTransient<IHostedService, DefaultUserInitializer>();
    }
}