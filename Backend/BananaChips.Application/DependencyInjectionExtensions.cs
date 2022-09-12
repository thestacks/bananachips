using System.Reflection;
using BananaChips.Application.Actions.Session.Commands;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using BananaChips.Application.Behaviors;

namespace BananaChips.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetAssembly(typeof(Login));
        return services.AddValidatorsFromAssembly(assembly)
            .AddMediatR(assembly)
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}