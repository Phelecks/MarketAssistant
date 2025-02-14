using BlockChainWeb3ProviderHelper.Interfaces;
using BlockChainWeb3ProviderHelper.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlockChainWeb3ProviderHelper;

public static class ConfigureServices
{
    public static void AddBlockChainWeb3ProviderDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<IWeb3ProviderService, Web3ProviderService>();
    }
}
