using BlockProcessor.Application.Interfaces;
using BlockProcessor.Application.RpcUrl.Queries.GetAllChains;
using LoggerService.Helpers;
using MediatR.Interfaces;

namespace BlockProcessor.Api.BackgroundServices;

public class BlockProcessorHostedService(IServiceScopeFactory serviceProvider, ILogger<BlockProcessorHostedService> logger) : BackgroundService
{
    private readonly IServiceScopeFactory _serviceProvider = serviceProvider;
    private readonly ILogger<BlockProcessorHostedService> _logger = logger;
    private readonly List<Nethereum.Signer.Chain> _blockProcessors = [];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalsInMinutes = 1;

        await Task.Delay(TimeSpan.FromMinutes(intervalsInMinutes), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<IRequestDispatcher>();
            
            var chains = await dispatcher.SendAsync(new GetAllChainsQuery(), stoppingToken);

            foreach (var chain in chains)
                if(!_blockProcessors.Contains(chain))
                    _ = Task.Run(() => StartBlockProcessorAsync(chain, stoppingToken), stoppingToken);

            _ = Task.Run(() => _logger.LogInformation(
                 eventId: EventTool.GetEventInformation(eventType: EventType.Information, eventName: $"{nameof(BlockProcessorHostedService)}"),
                 "{@HostedServiceName} is delaying for {@Delay} minutes.", nameof(BlockProcessorHostedService), intervalsInMinutes), stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(intervalsInMinutes), stoppingToken);
        }
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.Information, eventName: nameof(BlockProcessorHostedService)),
           "{@HostedServiceName} is starting.", nameof(BlockProcessorHostedService)), cancellationToken);

        await base.StartAsync(cancellationToken);
    }
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.Information, eventName: nameof(BlockProcessorHostedService)),
           "{@HostedServiceName} is stopping.", nameof(BlockProcessorHostedService)), cancellationToken);

        await base.StopAsync(cancellationToken);
    }

    async Task StartBlockProcessorAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        try
        {
            _blockProcessors.Add(chain);

            using var scope = _serviceProvider.CreateScope();
            var sender = scope.ServiceProvider.GetRequiredService<IRequestDispatcher>();
            var blockProcessor = scope.ServiceProvider.GetRequiredService<IBlockProcessorService>();
            await blockProcessor.StartAsync(chain, cancellationToken);
        }
        catch(Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.Exception, eventName: nameof(BlockProcessorHostedService)),
                exception, exception.Message), cancellationToken);
        }
        finally
        {
            _blockProcessors.RemoveAll(exp => exp == chain);
        }
    }
}