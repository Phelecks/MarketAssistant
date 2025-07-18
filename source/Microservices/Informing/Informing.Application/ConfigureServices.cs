using System.Reflection;
using BaseApplication;
using MediatR;
using MediatR.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Informing.Application;

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
