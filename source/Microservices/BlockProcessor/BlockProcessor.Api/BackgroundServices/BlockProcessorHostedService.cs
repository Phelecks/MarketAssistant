using System.Collections.Concurrent;
using BlockProcessor.Application.Interfaces;
using BlockProcessor.Application.RpcUrl.Queries.GetAllChains;
using LoggerService.Helpers;
using MediatR;

namespace BlockProcessor.Api.BackgroundServices;

public class BlockProcessorHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BlockProcessorHostedService> _logger;

    public BlockProcessorHostedService(IServiceProvider serviceProvider, ILogger<BlockProcessorHostedService> logger)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            var intervalsInMinutes = 1;

            try
            {
                await DoProcessAsync(stoppingToken);
            }
            catch (Exception exception)
            {
                _ = Task.Run(() => _logger.LogError(
                     eventId: EventTool.GetEventInformation(eventType: EventType.BlockProcessorBackgroundTasks, eventName: $"{nameof(BlockProcessorHostedService)}"),
                     exception, exception.Message, stoppingToken), stoppingToken);
            }


            _ = Task.Run(() => _logger.LogInformation(
                 eventId: EventTool.GetEventInformation(eventType: EventType.BlockProcessorBackgroundTasks, eventName: $"{nameof(BlockProcessorHostedService)}"),
                 "{@HostedServiceName} is delaying for {@Delay} minutes.", nameof(BlockProcessorHostedService), intervalsInMinutes), stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(intervalsInMinutes), stoppingToken);
        }
    }



    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.BlockProcessorBackgroundTasks, eventName: nameof(BlockProcessorHostedService)),
           "{@HostedServiceName} is starting.", nameof(BlockProcessorHostedService)), cancellationToken);

        await base.StartAsync(cancellationToken);
    }
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.BlockProcessorBackgroundTasks, eventName: nameof(BlockProcessorHostedService)),
           "{@HostedServiceName} is stopping.", nameof(BlockProcessorHostedService)), cancellationToken);

        await base.StopAsync(cancellationToken);
    }

    async Task DoProcessAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        var blockProcessor = scope.ServiceProvider.GetRequiredService<IBlockProcessorService>();

        var chains = await sender.Send(new GetAllChainsQuery(), cancellationToken);

        var exceptions = new ConcurrentBag<Exception>();
        var options = new ParallelOptions() { MaxDegreeOfParallelism = chains.Count, CancellationToken = cancellationToken };
        await Parallel.ForEachAsync(chains, options, async (chain, _) => 
        {
            try
            {
                await blockProcessor.StartAsync(chain, cancellationToken);
            }
            catch(Exception exception)
            {
                exceptions.Add(exception);
            }
        });

        while (true)
        {
            try
            {
                var blockProcessorTasksQuery =
                        from chain in chains
                        where true
                        select blockProcessor.StartAsync(chain, cancellationToken);
                var blockProcessorTasks = blockProcessorTasksQuery.ToList();
                while(blockProcessorTasks.Count != 0)
                {
                    var blockProcessorTask = await Task.WhenAny(blockProcessorTasks);
                    if (blockProcessorTask.IsFaulted) 
                    {
                        //raise error and do something
                    }

                    blockProcessorTasks.Remove(blockProcessorTask);
                }
            }
            catch
            {
                break;
            }
        }
    }
}