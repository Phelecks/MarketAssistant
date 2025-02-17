using BlockChainTransferHelper.Interfaces;
using BlockChainTransferHelper.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlockChainTransferHelper;

public static class ConfigureServices
{
    public static void AddBlockChainTransferDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<ITransferService, TransferService>();
    }
}
