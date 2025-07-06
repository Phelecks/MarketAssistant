using System.Reflection;
using CustomMediatR.Behaviors;
using CustomMediatR.Interfaces;
using CustomMediatR.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CustomMediatR;

public static class CustomMediatRConfiguration
{
    public static void AddCustomMediator(this IServiceCollection services)
    {
        // Register core dispatcher
        services.AddScoped<IRequestDistpacher, RequestDispatcher>();

        // Register all handlers
        services.AddHandlers();

        // Register all validators
        services.AddValidatorsFromAssemblies([Assembly.GetExecutingAssembly(), Assembly.GetCallingAssembly()]);

        // Register behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RetryBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
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
