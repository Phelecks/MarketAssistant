using Informing.Infrastructure.MassTransit.Consumers.Messages;
using MassTransit;
using MassTransitManager;
using MassTransitManager.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Informing.Infrastructure.MassTransit;

internal static class MassTransitConfiguration
{
    public static void AddMassTransitDependencyInjections(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            //Messages
            x.AddConsumer<UpdateInformingContactMessageConsumer>();
            x.AddConsumer<SendSignUpVerificationCodeMessageConsumer>();
            x.AddConsumer<CreateInformingContactMessageConsumer>();
            x.AddConsumer<SendGeneralVerificationCodeMessageConsumer>();
            x.AddConsumer<SubmitSystemErrorMessageConsumer>();
            x.AddConsumer<NotifyBetInitiatedMessageConsumer>();
            x.AddConsumer<NotifyBetConfirmedMessageConsumer>();
            x.AddConsumer<NotifyBetFailedMessageConsumer>();
            x.AddConsumer<NotifyBetStatusMessageConsumer>();
            x.AddConsumer<NotifyRewardPaidMessageConsumer>();
            x.AddConsumer<NotifyMatchOverviewMessageConsumer>();
            x.AddConsumer<NotifyMatchMinutesLeftMessageConsumer>();
            x.AddConsumer<NotifyTransferConfirmedMessageConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                var configService = context.GetRequiredService<IConfiguration>();
                var connectionString = configService.GetConnectionString("messaging");
                cfg.Host(connectionString);

                cfg.UseMessageRetry(r => r.Exponential(retryLimit: 10, minInterval: TimeSpan.FromMinutes(1), maxInterval: TimeSpan.FromMinutes(20), intervalDelta: TimeSpan.FromMinutes(2)));

                //Messages
                cfg.ReceiveEndpoint(Queues.CreateInformingContactMessageQueueName, e =>
                {
                    e.ConfigureConsumer<CreateInformingContactMessageConsumer>(context);
                });
                cfg.ReceiveEndpoint(Queues.UpdateInformingContactMessageQueueName, e =>
                {
                    e.ConfigureConsumer<UpdateInformingContactMessageConsumer>(context);
                });
                cfg.ReceiveEndpoint(Queues.SendSignUpVerificationCodeMessageQueueName, e =>
                {
                    e.ConfigureConsumer<SendSignUpVerificationCodeMessageConsumer>(context);
                });
                cfg.ReceiveEndpoint(Queues.SendGeneralVerificationCodeMessageQueueName, e =>
                {
                    e.ConfigureConsumer<SendGeneralVerificationCodeMessageConsumer>(context);
                });
                cfg.ReceiveEndpoint(Queues.SubmitSystemErrorMessageQueueName, e =>
                {
                    e.ConfigureConsumer<SubmitSystemErrorMessageConsumer>(context);
                });
                cfg.ReceiveEndpoint(Queues.NotifyBetInitiatedMessageQueueName, e =>
                {
                    e.ConfigureConsumer<NotifyBetInitiatedMessageConsumer>(context);
                });
                cfg.ReceiveEndpoint(Queues.NotifyBetConfirmedMessageQueueName, e =>
                {
                    e.ConfigureConsumer<NotifyBetConfirmedMessageConsumer>(context);
                });
                cfg.ReceiveEndpoint(Queues.NotifyBetFailedMessageQueueName, e =>
                {
                    e.ConfigureConsumer<NotifyBetFailedMessageConsumer>(context);
                });
                cfg.ReceiveEndpoint(Queues.NotifyBetStatusMessageQueueName, e =>
                {
                    e.ConfigureConsumer<NotifyBetStatusMessageConsumer>(context);
                });
                cfg.ReceiveEndpoint(Queues.NotifyRewardPaidMessageQueueName, e =>
                {
                    e.ConfigureConsumer<NotifyRewardPaidMessageConsumer>(context);
                });
                cfg.ReceiveEndpoint(Queues.NotifyMatchOverviewMessageQueueName, e =>
                {
                    e.ConfigureConsumer<NotifyMatchOverviewMessageConsumer>(context);
                });
                cfg.ReceiveEndpoint(Queues.NotifyMatchMinutesLeftMessageQueueName, e =>
                {
                    e.ConfigureConsumer<NotifyMatchMinutesLeftMessageConsumer>(context);
                });
                cfg.ReceiveEndpoint(Queues.NotifyTransferConfirmedMessageQueueName, e =>
                {
                    e.ConfigureConsumer<NotifyTransferConfirmedMessageConsumer>(context);
                });
            });
        });
        services.AddMassTransitBaseDependencyInjections();
    }
}