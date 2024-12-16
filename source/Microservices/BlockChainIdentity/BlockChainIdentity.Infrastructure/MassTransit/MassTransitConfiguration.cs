using BlockChainIdentity.Infrastructure.MassTransit.Consumers.Events;
using BlockChainIdentity.Infrastructure.MassTransit.Consumers.Messages;
using MassTransit;
using MassTransitManager;
using MassTransitManager.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlockChainIdentity.Infrastructure.MassTransit;

internal static class MassTransitConfiguration
{
    public static void AddMassTransitDependencyInjections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateResourceMessageConsumer>();
            x.AddConsumer<BlockChainIdentityProcessorBaseParameterUpdatedEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetValue<string>("RABBITMQ_SERVER"), host =>
                {
                    host.Username(configuration.GetValue<string>("RABBITMQ_USERNAME"));
                    host.Password(configuration.GetValue<string>("RABBITMQ_PASSWORD"));
                });

                cfg.UseMessageRetry(r => r.Exponential(retryLimit: 10, minInterval: TimeSpan.FromMinutes(1), maxInterval: TimeSpan.FromMinutes(20), intervalDelta: TimeSpan.FromMinutes(2)));

                cfg.ReceiveEndpoint(Queues.CreateResourceMessageQueueName, e =>
                {
                    e.ConfigureConsumer<CreateResourceMessageConsumer>(context);
                });

                cfg.ReceiveEndpoint(Queues.BlockChainIdentityBaseParameterUpdatedEventQueueName, e =>
                {
                    e.ConfigureConsumer<BlockChainIdentityProcessorBaseParameterUpdatedEventConsumer>(context);
                });
            });
        });
        services.AddMassTransitBaseDependencyInjections();
    }
}