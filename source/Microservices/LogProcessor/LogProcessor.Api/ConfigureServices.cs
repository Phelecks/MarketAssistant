using BaseApi;
using LogProcessor.Api.BackgroundServices;

namespace LogProcessor.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services.AddBaseApiServices(builder, sqlConnectionName: "logprocessordb", redisDistributedCacheConnectionName: "cache", rabbitMQConnectionName: "messaging");

        builder.Services.AddHostedService<LogProcessorHostedService>();

        return services;
    }

    public static void AddConfiguration(this WebApplication app)
    {
        app.AddBaseApiConfiguration();
    }
}
