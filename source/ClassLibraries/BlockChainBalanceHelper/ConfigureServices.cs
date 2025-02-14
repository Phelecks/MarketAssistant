using BlockChainBalanceHelper.Interfaces;
using BlockChainBalanceHelper.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlockChainBalanceHelper;

public static class ConfigureServices
{
    public static void AddBlockChainBalanceDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<IBalanceService, BalanceService>();
    }
}
