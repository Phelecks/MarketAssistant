using System.Reflection;
using BaseApplication;
using BlockChainQueryHelper;
using BlockChainWeb3ProviderHelper;
using Microsoft.Extensions.DependencyInjection;

namespace LogProcessor.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddBaseApplicationServices();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(configuration: configuration =>
        {
            configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });

        services.AddBlockChainWeb3ProviderDependencyInjections();
        services.AddBlockChainQueryDependencyInjections();

        return services;
    }
}
