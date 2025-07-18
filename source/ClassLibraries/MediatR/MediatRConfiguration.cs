using System.Reflection;
using MediatR.Interfaces;
using MediatR.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR;

public static class AddDomainNotificationConfiguration
{
    public static void AddMediatR(this IServiceCollection services)
    {
        // Register core dispatcher
        services.AddScoped<IRequestDispatcher, RequestDispatcher>();

        // Register all handlers
        services.AddHandlers();

        // Register all validators
        services.AddValidatorsFromAssemblies([Assembly.GetExecutingAssembly(), Assembly.GetCallingAssembly()]);
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        var assembly = Assembly.GetEntryAssembly();

        if (assembly is null) return services;
         
        foreach (var type in assembly.GetTypes())
        {
            var interfaces = type.GetInterfaces().Where(i =>
                i.IsGenericType && (
                    i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                    i.GetGenericTypeDefinition() == typeof(INotificationHandler<>)));

            foreach (var iface in interfaces)
            {
                services.AddTransient(iface, type);
            }
        }

        return services;
    }
}
