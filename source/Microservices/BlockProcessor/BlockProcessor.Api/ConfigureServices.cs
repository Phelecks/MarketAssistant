using BlockProcessor.Api.BackgroundServices;
using BaseApi;

namespace BlockProcessor.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services.AddBaseApiServices(builder, sqlConnectionName: "blockprocessordb", redisDistributedCacheConnectionName: "cache", rabbitMQConnectionName: "messaging");

        builder.Services.AddHostedService<BlockProcessorHostedService>();

        return services;
    }

    public static void AddConfiguration(this WebApplication app)
    {
        app.AddBaseApiConfiguration();
    }
}
