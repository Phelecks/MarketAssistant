using System.Reflection;
using BaseApplication;
using BlockChainQueryHelper;
using BlockChainWeb3ProviderHelper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BlockProcessor.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddBaseApplicationServices();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR();

        services.AddBlockChainWeb3ProviderDependencyInjections();
        services.AddBlockChainQueryDependencyInjections();

        return services;
    }
}
