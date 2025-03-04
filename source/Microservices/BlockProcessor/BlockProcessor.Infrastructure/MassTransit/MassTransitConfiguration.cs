using BlockProcessor.Infrastructure.MassTransit.Consumers.Events;
using MassTransit;
using MassTransitManager;
using MassTransitManager.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlockProcessor.Infrastructure.MassTransit;

internal static class MassTransitConfiguration
{
    public static void AddMassTransitDependencyInjections(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateWalletAddressEventConsumer>();
            x.AddConsumer<DeleteWalletAddressEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var configService = context.GetRequiredService<IConfiguration>();
                var connectionString = configService.GetConnectionString("messaging");
                cfg.Host(connectionString);

                cfg.UseMessageRetry(r => r.Exponential(retryLimit: 10, minInterval: TimeSpan.FromMinutes(1), maxInterval: TimeSpan.FromMinutes(20), intervalDelta: TimeSpan.FromMinutes(2)));

                cfg.ReceiveEndpoint(Queues.BlockProcessorCreateWalletAddressEventQueueName, e =>
                {
                    e.ConfigureConsumer<CreateWalletAddressEventConsumer>(context, configure =>
                    {
                        configure.ConcurrentMessageLimit = 1;
                    });
                });

                cfg.ReceiveEndpoint(Queues.BlockProcessorDeleteWalletAddressEventQueueName, e =>
                {
                    e.ConfigureConsumer<DeleteWalletAddressEventConsumer>(context, configure =>
                    {
                        configure.ConcurrentMessageLimit = 1;
                    });
                });
            });
        });
        services.AddMassTransitBaseDependencyInjections();
    }
}