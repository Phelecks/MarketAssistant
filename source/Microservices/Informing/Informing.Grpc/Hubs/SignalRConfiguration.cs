using Informing.Application.Interfaces;
using StackExchange.Redis;
using Microsoft.AspNetCore.SignalR;
using Informing.Grpc.Helpers;

namespace Informing.Grpc.Hubs;

public static class SignalRConfiguration
{
    /// <summary>
    /// Add dependency injections
    /// </summary>
    /// <param name="services"></param>
    public static void AddSignalRServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IUserIdProvider, CustomSignalRUserIdProvider>();

        services.AddSignalR()
            .AddHubOptions<GameHub>(options =>
            {
                options.ClientTimeoutInterval = TimeSpan.FromMinutes(10);
            })
            .AddStackExchangeRedis(redisConnectionString: "cache")
            .AddMessagePackProtocol();

        services.AddScoped<IGameHubProxy, GameHubProxy>();
    }

    public static void AddSignalRConfigurations(this WebApplication app)
    {
        app.MapHub<GameHub>("/hubs/game", options =>
        {
            options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
        });
    }
}