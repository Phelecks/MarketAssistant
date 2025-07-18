using System.Reflection;
using BaseApplication;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BlockChainIdentity.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddBaseApplicationServices();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR();

        //Add custom Behavior

        return services;
    }
}
