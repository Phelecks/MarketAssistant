using System.Numerics;
using BaseApplication.Interfaces;
using BlockProcessor.Application.BlockProgress.Commands.MarkBlockAsProcessed;
using BlockProcessor.Application.BlockProgress.Queries.GetLastProcessedBlock;
using BlockProcessor.Application.Interfaces;
using BlockProcessor.Application.WalletAddress.Queries.GetAllAddresses;
using DistributedProcessManager.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nethereum.BlockchainProcessing;
using Nethereum.BlockchainProcessing.BlockProcessing;
using Nethereum.BlockchainProcessing.Orchestrator;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.Blocks;
using Nethereum.Util;
using Polly;

namespace BlockProcessor.Infrastructure.Services;

public class BlockProcessorService(IServiceProvider serviceProvider, ILogger<BlockProcessorService> logger, IDateTimeService dateTimeService) : IBlockProcessorService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<BlockProcessorService> _logger = logger;
    private readonly IDateTimeService _dateTimeService = dateTimeService;

    private ResiliencePipeline _pollyPipeline;
    private readonly AddressUtil _addressUtil =  new();
    private List<string> _addresses = new();

    public async Task StartAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        _pollyPipeline = scope.ServiceProvider.GetRequiredKeyedService<ResiliencePipeline>(ExecutorManager.Helpers.PipelineHelper.RetryEverythingForever);

        _addresses = await sender.Send(new GetAllAddressesQuery(), cancellationToken);

        var lastProcessBlock = await sender.Send(new GetLastProcessedBlockQuery(chain), cancellationToken);
        var distributedBlockChainProgressRepository = scope.ServiceProvider.GetRequiredService<DistributedBlockChainProgressRepository>();
        var distributedBlockChainProgressRepositoryInstance = await distributedBlockChainProgressRepository.
            GetInstanceAsync(chain: chain, cacheKey: "BlockProcessor_BlockProgress", lastBlockNumber: lastProcessBlock, withCache: true);
        distributedBlockChainProgressRepositoryInstance.BlockProcessedEventHandler += async (sender, @event) => await RaiseBlockProcessedEvent(@event.Chain, @event.BlockNumber, cancellationToken);

        await ProcessAsync(chain, distributedBlockChainProgressRepositoryInstance, cancellationToken);
    }

    public Task StopAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RestartAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }




    private async Task ProcessAsync(Nethereum.Signer.Chain chain, DistributedBlockChainProgressRepository distributedBlockChainProgressRepository,
        CancellationToken cancellationToken)
    {
        var minimumBlockConfirmations = await GetMinimumBlockConfirmationsAsync(cancellationToken);
        var waitInterval = await GetWaitIntervalAsync(cancellationToken);

        var web3 = await _web3ProviderService.CreateWeb3Async(chainId, cancellationToken);

        var blockProcessingSteps = new BlockProcessingSteps();

        //Filter destination address only
        blockProcessingSteps.TransactionStep.SetMatchCriteria(criteria =>
            _addresses is not null &&
            _addresses.Any(address => _addressUtil.AreAddressesTheSame(address, criteria.Transaction.To) || _addressUtil.AreAddressesTheSame(address, criteria.Transaction.From))
           );
        blockProcessingSteps.FilterLogStep.SetMatchCriteria(criteria =>
            criteria.IsLogForEvent<Nethereum.Contracts.Standards.ERC20.ContractDefinition.TransferEventDTO>()
            &&
            _addresses is not null &&
            _addresses.Any(address => criteria.IsTo(address) || _addressUtil.AreAddressesTheSame(address, criteria.Transaction.From)));
        blockProcessingSteps.FilterLogStep.SetMatchCriteria(criteria =>
            criteria.IsLogForEvent<Nethereum.Contracts.Standards.ERC721.ContractDefinition.TransferEventDTO>()
            &&
            _addresses is not null &&
            _addresses.Any(address => criteria.IsTo(address) || _addressUtil.AreAddressesTheSame(address, criteria.Transaction.From)));

        blockProcessingSteps.TransactionReceiptStep.AddProcessorHandler(async (transactionReceipt) => await ProcessTransactionAsync(chainId, transactionReceipt, cancellationToken));

        IBlockchainProcessingOrchestrator orchestrator = new BlockCrawlOrchestrator(ethApi: web3.Eth, blockProcessingSteps: blockProcessingSteps);

        //create an in-memory context and repository factory 
        var progressRepository = distributedBlockChainProgressRepository;
        
        // this strategy is applied while waiting for block confirmations
        // it will apply a wait to allow the chain to add new blocks
        // the wait duration is dependant on the number of retries
        // feel free to implement your own
        IWaitStrategy waitForBlockConfirmationsStrategy = new CustomWaitStrategy();

        // this retrieves the current block on the chain (the most recent block)
        // it determines the next block to process ensuring it is within the min block confirmations
        // in the scenario where processing is up to date with the chain (i.e. processing very recent blocks)
        // it will apply a wait until the minimum block confirmations is met
        ILastConfirmedBlockNumberService lastConfirmedBlockNumberService =
            new LastConfirmedBlockNumberService(
                web3.Eth.Blocks.GetBlockNumber,
                waitForBlockConfirmationsStrategy,
                minimumBlockConfirmations: uint.Parse(minimumBlockConfirmations.ToString()),
                log: _logger);

        // instantiate the main processor
        var processor = new BlockchainProcessor(
            blockchainProcessingOrchestrator: orchestrator,
            blockProgressRepository: progressRepository,
            lastConfirmedBlockNumberService: lastConfirmedBlockNumberService,
            log: _logger);


        // Execute the pipeline asynchronously
        await _pollyPipeline.ExecuteAsync(async (ct) => {
            await processor.ExecuteAsync(
               waitInterval: waitInterval,
               cancellationToken: cancellationToken);
        }, cancellationToken);
    }


    /// <summary>
    /// Fires when a block processed
    /// </summary>
    private async Task RaiseBlockProcessedEvent(Nethereum.Signer.Chain chain, BigInteger blockNumber, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        
        // Mark block as processed
        await sender.Send(new MarkBlockAsProcessedCommand(chain, blockNumber), cancellationToken);

        // Update addresses
    }


    private class CustomWaitStrategy : IWaitStrategy
    {
        private static readonly int[] WaitIntervals = [1000, 5000, 10000, 20000, 30000, 40000, 50000, 60000];

        public Task ApplyAsync(uint retryCount)
        {
            var intervalMs = retryCount >= WaitIntervals.Length ? WaitIntervals[^1] : WaitIntervals[retryCount];

            return Task.Delay(intervalMs);
        }
    }
}