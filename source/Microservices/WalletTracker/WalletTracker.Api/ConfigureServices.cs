using BaseApi;
using WalletTracker.Api.BackgroundServices;

namespace WalletTracker.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services.AddBaseApiServices(builder, sqlConnectionName: "wallettrackerdb", redisDistributedCacheConnectionName: "cache", rabbitMQConnectionName: "messaging");

        builder.Services.AddHostedService<MainHostedService>();

        return services;
    }

    public static void AddConfiguration(this WebApplication app)
    {
        app.AddBaseApiConfiguration();
    }
}
