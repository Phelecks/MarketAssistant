using MassTransitManager.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransitManager;

public static class Configurations
{
    public static void AddMassTransitBaseDependencyInjections(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IMassTransitService, MassTransitService>();
    }
}