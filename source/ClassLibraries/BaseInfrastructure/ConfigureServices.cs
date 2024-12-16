using BaseApplication.Interfaces;
using BaseInfrastructure.Persistence.Interceptors;
using BaseInfrastructure.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace BaseInfrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddBaseInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IRandomGeneratorService, RandomGeneratorService>();
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddSingleton<ICypherService, CypherService>();

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        return services;
    }
}
