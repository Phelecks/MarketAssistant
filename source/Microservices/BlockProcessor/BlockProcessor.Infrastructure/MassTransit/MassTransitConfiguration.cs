using BaseApplication.Exceptions;
using BlockProcessor.Infrastructure.MassTransit.Consumers.Events;
using MassTransit;
using MassTransitManager;
using MassTransitManager.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlockProcessor.Infrastructure.MassTransit;

internal static class MassTransitConfiguration
{
    public static void AddMassTransitDependencyInjections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<CreateWalletAddressEventConsumer>();
            x.AddConsumer<DeleteWalletAddressEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetValue<string>("RABBITMQ_SERVER"), host =>
                {
                    var username = configuration.GetValue<string>("RABBITMQ_USERNAME");
                    var password = configuration.GetValue<string>("RABBITMQ_PASSWORD");
                    if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                        throw new NotFoundException("RabbitMQ username or password is not set in the configuration.");
                    
                    host.Username(username);
                    host.Password(password);
                });

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