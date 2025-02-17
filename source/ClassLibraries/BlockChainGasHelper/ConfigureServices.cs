using BlockChainGasHelper.Interfaces;
using BlockChainGasHelper.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlockChainGasHelper;

public static class ConfigureServices
{
    public static void AddBlockChainGasDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<IGasService, GasService>();
    }
}
