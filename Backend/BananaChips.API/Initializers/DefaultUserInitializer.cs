using BananaChips.Application.Exceptions;
using BananaChips.Domain.Entities;
using BananaChips.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BananaChips.API.Initializers;

public class DefaultUserInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DefaultUserInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DefaultUserInitializer>>();
        logger.LogInformation("{InitializerName} Starting", nameof(DefaultUserInitializer));

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var authenticationSettings = new AuthenticationSettings().LoadFromConfiguration(configuration);
        var administratorSettings = authenticationSettings.DefaultAdministrator;
        try
        {
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.Email == administratorSettings.Email);
            if (user != null)
            {
                logger.LogInformation("{InitializerName} User already exists. Exiting...", nameof(DefaultUserInitializer));
                return;
            }

            logger.LogInformation("{InitializerName} User does not exist. Creating...", nameof(DefaultUserInitializer));
            user = new User
            {
                Email = administratorSettings.Email,
                UserName = administratorSettings.Email,
                FirstName = administratorSettings.FirstName,
                LastName = administratorSettings.LastName,
                EmailConfirmed = true
            };

            var creationResult = await userManager.CreateAsync(user, administratorSettings.Password);
            if (!creationResult.Succeeded)
                throw new InitializerException(string.Join(";", creationResult.Errors.Select(e => e.Description)));
            logger.LogInformation("{InitializerName} User has been successfully created. Exiting...", nameof(DefaultUserInitializer));
        }
        catch(InitializerException initializerException)
        {
            logger.LogError(initializerException, "{InitializerName} Critical error occured. {InitializerErrorMessage}", nameof(DefaultUserInitializer), initializerException.Message);
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}