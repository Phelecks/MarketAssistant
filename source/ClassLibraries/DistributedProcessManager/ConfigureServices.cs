using DistributedProcessManager.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedProcessManager;

public static class ConfigureServices
{
    public static void AddDistributedBlockProgressRepositoryDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<DistributedBlockChainProgressRepository>();
    }
}
