using System.Reflection;
using BaseApplication.Behavior;
using BaseApplication.Behaviors;
using CacheManager;
using ExecutorManager;
using MediatR;
using MediatR.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApplication;

public static class ConfigureServices
{
    public static IServiceCollection AddBaseApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
       
        // services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        // services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly());

        services.AddMediatR();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

        services.AddExecutorManagerDependencyInjections();

        services.AddCacheManagerDependencyInjections();

        return services;
    }
}
