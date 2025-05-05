using System.Reflection;
using BaseApplication;
using Microsoft.Extensions.DependencyInjection;

namespace BlockChainIdentity.Application;

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

        //Add custom behaviour

        return services;
    }
}
