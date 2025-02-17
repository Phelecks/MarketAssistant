using BlockChainHDWalletHelper.Interfaces;
using BlockChainHDWalletHelper.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlockChainHDWalletHelper;

public static class ConfigureServices
{
    public static void AddBlockChainHDWalletDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<IHdWalletService, HdWalletService>();
    }
}
