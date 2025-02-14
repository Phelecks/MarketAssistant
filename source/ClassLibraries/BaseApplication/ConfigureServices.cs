using System.Reflection;
using BaseApplication.Behaviour;
using CacheManager;
using ExecutorManager;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApplication;

public static class ConfigureServices
{
    public static IServiceCollection AddBaseApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
       
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly());
        services.AddMediatR(configuration: configuration =>
        {
            configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        services.AddExecutorManagerDependencyInjections();

        services.AddCacheManagerDependencyInjections();

        return services;
    }
}
