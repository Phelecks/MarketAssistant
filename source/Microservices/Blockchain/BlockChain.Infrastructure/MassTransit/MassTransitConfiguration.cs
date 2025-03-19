﻿using BlockChain.Infrastructure.MassTransit.Consumers.Messages;
using MassTransit;
using MassTransitManager;
using MassTransitManager.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlockChain.Infrastructure.MassTransit;

internal static class MassTransitConfiguration
{
    public static void AddMassTransitDependencyInjections(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<TransferConfirmedMessageConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var configService = context.GetRequiredService<IConfiguration>();
                var connectionString = configService.GetConnectionString("messaging");
                cfg.Host(connectionString);

                cfg.UseMessageRetry(r => r.Exponential(retryLimit: 10, minInterval: TimeSpan.FromMinutes(1), maxInterval: TimeSpan.FromMinutes(20), intervalDelta: TimeSpan.FromMinutes(2)));

                cfg.ReceiveEndpoint(Queues.TransferConfirmedMessageQueueName, e =>
                {
                    e.ConfigureConsumer<TransferConfirmedMessageConsumer>(context);
                });
            });
        });
        services.AddMassTransitBaseDependencyInjections();
    }
}