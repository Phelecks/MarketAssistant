using System.Reflection;
using MassTransit;
using MassTransitManager.Helpers;
using Microsoft.EntityFrameworkCore;
using SagaOrchestrator.Persistence;
using SagaOrchestrator.StatemachineInstances;
using SagaOrchestrator.StateMachines;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMassTransit(options =>
    {
        options.AddSagaStateMachine<TransferStateMachine, TransferStateMachineInstance>()
            .EntityFrameworkRepository(entityFrameworkOptions =>
            {
                entityFrameworkOptions.ConcurrencyMode = ConcurrencyMode.Pessimistic; // or use Optimistic, which requires RowVersion

                entityFrameworkOptions.AddDbContext<DbContext, ApplicationDbContext>((serviceProvider, dbContextOptions) =>
                {
                    var connectionString = builder.Configuration.GetConnectionString("orchestrationdb");
                    dbContextOptions.UseSqlServer(connectionString, m =>
                    {
                        m.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        m.MigrationsHistoryTable($"__{nameof(ApplicationDbContext)}");
                    });
                });
            });

        options.UsingRabbitMq((context, config) =>
        {
            config.Host(builder.Configuration.GetConnectionString("messaging"));

            //Events
            //config.ReceiveEndpoint(Queues.NotifyTransferConfirmedMessageQueueName, e => { e.ConfigureSaga<TransferStateMachineInstance>(context); });
        });


        var entryAssembly = Assembly.GetEntryAssembly();

        options.AddConsumers(entryAssembly);
        options.AddSagaStateMachines(entryAssembly);
        options.AddSagas(entryAssembly);
        options.AddActivities(entryAssembly);

        options.UsingInMemory((context, cfg) =>
        {
            cfg.ConfigureEndpoints(context);
        });
    });

var host = builder.Build();
await host.RunAsync();
