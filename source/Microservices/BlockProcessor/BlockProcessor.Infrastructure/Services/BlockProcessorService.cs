using System.Collections.Concurrent;
using BaseApplication.Interfaces;
using BlockProcessor.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nethereum.Signer;

namespace BlockProcessor.Infrastructure.Services;

public class BlockProcessorService(IServiceProvider serviceProvider, ILogger<BlockProcessorService> logger, IDateTimeService dateTimeService) : IBlockProcessorService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<BlockProcessorService> _logger = logger;
    private readonly IDateTimeService _dateTimeService = dateTimeService;


    public async Task StartAsync(Chain chain, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        
    }

    public Task StopAsync(Chain chain, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RestartAsync(Chain chain, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}