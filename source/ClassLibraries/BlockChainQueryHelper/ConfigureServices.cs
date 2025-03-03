using BlockChainQueryHelper.Interfaces;
using BlockChainQueryHelper.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlockChainQueryHelper;

public static class ConfigureServices
{
    public static void AddBlockChainQueryDependencyInjections(this IServiceCollection services)
    {
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IBlockService, BlockService>();
    }
}
