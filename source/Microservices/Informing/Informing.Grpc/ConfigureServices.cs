using BaseApi;
using Informing.Grpc.Hubs;

namespace Informing.Grpc;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services.AddBaseApiServices(builder, sqlConnectionName: "informingdb", redisDistributedCacheConnectionName: "cache", rabbitMQConnectionName: "messaging");

        builder.Services.AddSignalRServices(builder.Configuration.GetConnectionString("cache"));

        return services;
    }

    public static void AddConfiguration(this WebApplication app)
    {
        app.AddBaseApiConfiguration();

        app.AddSignalRConfigurations();
    }
}
