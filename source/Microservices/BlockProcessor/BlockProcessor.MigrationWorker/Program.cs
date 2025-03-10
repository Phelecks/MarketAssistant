using BlockProcessor.Infrastructure.Persistence;
using BlockProcessor.MigrationWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddSqlServerDbContext<ApplicationDbContext>("blockprocessordb");

var host = builder.Build();
await host.RunAsync();
