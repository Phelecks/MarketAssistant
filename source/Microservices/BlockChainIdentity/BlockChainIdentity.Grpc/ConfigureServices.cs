using BaseApi;

namespace BlockChainIdentity.Grpc;

public static class ConfigureServices
{
    public static IServiceCollection AddServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services.AddBaseApiServices(builder, sqlConnectionName: "identitydb", redisDistributedCacheConnectionName: "cache", rabbitMQConnectionName: "messaging");

        return services;
    }

    public static void AddConfiguration(this WebApplication app)
    {
        app.AddBaseApiConfiguration();
    }
}
