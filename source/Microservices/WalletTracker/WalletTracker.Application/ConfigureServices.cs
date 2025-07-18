using System.Reflection;
using BaseApplication;
using BlockChainBalanceHelper;
using BlockChainGasHelper;
using BlockChainHDWalletHelper;
using BlockChainTransferHelper;
using BlockChainWeb3ProviderHelper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace WalletTracker.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddBaseApplicationServices();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR();

        //Add custom Behavior

        services.AddBlockChainGasDependencyInjections();
        services.AddBlockChainBalanceDependencyInjections();
        services.AddBlockChainHDWalletDependencyInjections();
        services.AddBlockChainTransferDependencyInjections();
        services.AddBlockChainWeb3ProviderDependencyInjections();

        return services;
    }
}
