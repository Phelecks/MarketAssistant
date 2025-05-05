using BaseApi;

namespace BlockChain.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services.AddBaseApiServices(builder, sqlConnectionName: "blockchaindb", redisDistributedCacheConnectionName: "cache", rabbitMQConnectionName: "messaging");

        return services;
    }

    public static void AddConfiguration(this WebApplication app)
    {
        app.AddBaseApiConfiguration();
    }
}
