using CacheManager.Interfaces;
using CacheManager.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CacheManager;

public static class CacheManagerConfiguration
{
    public static void AddCacheManagerDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<IDistributedLockService, DistributedLockService>();
        services.AddScoped(typeof(IDistributedLockService<>), typeof(DistributedLockService<>));
    }
}
