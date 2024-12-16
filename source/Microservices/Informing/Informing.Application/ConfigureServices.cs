using System.Reflection;
using BaseApplication;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Informing.Application;

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
