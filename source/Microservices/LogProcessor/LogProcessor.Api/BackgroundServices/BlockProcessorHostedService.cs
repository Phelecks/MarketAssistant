using LogProcessor.Application.Interfaces;
using LogProcessor.Application.RpcUrl.Queries.GetAllChains;
using LoggerService.Helpers;
using MediatR;

namespace LogProcessor.Api.BackgroundServices;

public class LogProcessorHostedService(IServiceScopeFactory serviceProvider, ILogger<LogProcessorHostedService> logger) : BackgroundService
{
    private readonly IServiceScopeFactory _serviceProvider = serviceProvider;
    private readonly ILogger<LogProcessorHostedService> _logger = logger;
    private readonly List<Nethereum.Signer.Chain> _logProcessors = [];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalsInMinutes = 1;

        await Task.Delay(TimeSpan.FromMinutes(intervalsInMinutes), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var sender = scope.ServiceProvider.GetRequiredService<ISender>();
            
            var chains = await sender.Send(new GetAllChainsQuery(), stoppingToken);

            foreach (var chain in chains)
                if(!_logProcessors.Contains(chain))
                    _ = Task.Run(() => StartLogProcessorAsync(chain, stoppingToken), stoppingToken);

            _ = Task.Run(() => _logger.LogInformation(
                 eventId: EventTool.GetEventInformation(eventType: EventType.LogProcessorBackgroundTasks, eventName: $"{nameof(LogProcessorHostedService)}"),
                 "{@HostedServiceName} is delaying for {@Delay} minutes.", nameof(LogProcessorHostedService), intervalsInMinutes), stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(intervalsInMinutes), stoppingToken);
        }
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.LogProcessorBackgroundTasks, eventName: nameof(LogProcessorHostedService)),
           "{@HostedServiceName} is starting.", nameof(LogProcessorHostedService)), cancellationToken);

        await base.StartAsync(cancellationToken);
    }
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.LogProcessorBackgroundTasks, eventName: nameof(LogProcessorHostedService)),
           "{@HostedServiceName} is stopping.", nameof(LogProcessorHostedService)), cancellationToken);

        await base.StopAsync(cancellationToken);
    }

    async Task StartLogProcessorAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        try
        {
            _logProcessors.Add(chain);

            using var scope = _serviceProvider.CreateScope();
            var sender = scope.ServiceProvider.GetRequiredService<ISender>();
            var logProcessors = scope.ServiceProvider.GetRequiredService<ILogProcessorService>();
            await logProcessors.StartAsync(chain, cancellationToken);
        }
        catch(Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.LogProcessorBackgroundTasks, eventName: nameof(LogProcessorHostedService)),
                exception, exception.Message), cancellationToken);
        }
        finally
        {
            _logProcessors.RemoveAll(exp => exp == chain);
        }
    }
}