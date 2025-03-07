using System.Numerics;
using BaseApplication.Interfaces;
using BlockChainWeb3ProviderHelper.Interfaces;
using BlockProcessor.Application.BlockProgress.Commands.MarkBlockAsProcessed;
using BlockProcessor.Application.BlockProgress.Queries.GetLastProcessedBlock;
using BlockProcessor.Application.Interfaces;
using BlockProcessor.Application.RpcUrl.Queries.GetMinimumBlockOfConfirmation;
using BlockProcessor.Application.RpcUrl.Queries.GetRpcUrl;
using BlockProcessor.Application.RpcUrl.Queries.GetWaitIntervalOfBlockProgress;
using BlockProcessor.Application.Transfer.Commands.InitiateTransfer;
using BlockProcessor.Application.WalletAddress.Queries.GetAllAddresses;
using DistributedProcessManager.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Nethereum.BlockchainProcessing;
using Nethereum.BlockchainProcessing.BlockProcessing;
using Nethereum.BlockchainProcessing.Orchestrator;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.Blocks;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;

namespace BlockProcessor.Infrastructure.Services;

public class BlockProcessorService(ISender sender, ILogger<BlockProcessorService> logger, 
    IWeb3ProviderService web3ProviderService, 
    DistributedBlockChainProgressRepository distributedBlockChainProgressRepository, IDateTimeService dateTimeService) : IBlockProcessorService
{
    private readonly ISender _sender = sender;
    private readonly ILogger<BlockProcessorService> _logger = logger;
    private readonly IWeb3ProviderService _web3ProviderService = web3ProviderService;
    private readonly IDateTimeService _dateTimeService = dateTimeService;
    private readonly AddressUtil _addressUtil =  new();
    private readonly DistributedBlockChainProgressRepository _distributedBlockChainProgressRepository = distributedBlockChainProgressRepository;

    private List<string> _addresses = [];
    public async Task StartAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        var lastProcessBlock = await _sender.Send(new GetLastProcessedBlockQuery(chain), cancellationToken);
        var distributedBlockChainProgressRepositoryInstance = await _distributedBlockChainProgressRepository.
            GetInstanceAsync(chain: chain, cacheKey: "BlockProcessor_BlockProgress", lastBlockNumber: lastProcessBlock, withCache: true);
        distributedBlockChainProgressRepositoryInstance.BlockProcessedEventHandler += async (sender, @event) => await RaiseBlockProcessedEvent(@event.Chain, @event.BlockNumber, cancellationToken);

        var minimumBlockConfirmations = await _sender.Send(new GetMinimumBlockOfConfirmationQuery(chain), cancellationToken);
        var waitInterval = await _sender.Send(new GetWaitIntervalOfBlockProgressQuery(chain), cancellationToken);
        var rpcUrl = await _sender.Send(new GetRpcUrlQuery(chain), cancellationToken);
        var web3 = _web3ProviderService.CreateWeb3(chain, rpcUrl);
        
        _addresses = await _sender.Send(new GetAllAddressesQuery(), cancellationToken);
        
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

        blockProcessingSteps.TransactionReceiptStep.AddProcessorHandler(async (transactionReceipt) => await ProcessTransactionAsync(chain, transactionReceipt, cancellationToken));

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

        await processor.ExecuteAsync(
               waitInterval: waitInterval,
               cancellationToken: cancellationToken);
    }

    public Task StopAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RestartAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Fires when a block processed
    /// </summary>
    private async Task RaiseBlockProcessedEvent(Nethereum.Signer.Chain chain, BigInteger blockNumber, CancellationToken cancellationToken)
    {
        // Mark block as processed
        await _sender.Send(new MarkBlockAsProcessedCommand(chain, blockNumber), cancellationToken);

        // Update addresses
        _addresses = await _sender.Send(new GetAllAddressesQuery(), cancellationToken);
    }

    private async Task ProcessTransactionAsync(Nethereum.Signer.Chain chain, TransactionReceiptVO transactionReceiptVO, CancellationToken cancellationToken)
    {
        if(transactionReceiptVO.Succeeded)
        {
            var erc20TransferLogs = transactionReceiptVO.TransactionReceipt.Logs?.DecodeAllEvents<Nethereum.Contracts.Standards.ERC20.ContractDefinition.TransferEventDTO>();
            var erc721TransferLogs = transactionReceiptVO.TransactionReceipt.Logs?.DecodeAllEvents<Nethereum.Contracts.Standards.ERC721.ContractDefinition.TransferEventDTO>();
                        
            await _sender.Send(new InitiateTransferCommand(
                Hash: transactionReceiptVO.TransactionHash, From: transactionReceiptVO.TransactionReceipt.From.ConvertToEthereumChecksumAddress(), To: transactionReceiptVO.TransactionReceipt.To.ConvertToEthereumChecksumAddress(), 
                Value: transactionReceiptVO.Transaction.Value, GasUsed: transactionReceiptVO.TransactionReceipt.GasUsed.Value, EffectiveGasPrice: transactionReceiptVO.TransactionReceipt.EffectiveGasPrice.Value,
                CumulativeGasUsed: transactionReceiptVO.TransactionReceipt.CumulativeGasUsed.Value, BlockNumber: transactionReceiptVO.BlockNumber.Value, ConfirmedDatetime: _dateTimeService.ConvertFromUnixTimestamp(transactionReceiptVO.BlockTimestamp), Chain: chain, 
                Erc20Transfers: erc20TransferLogs?.Select(eventLog => new Erc20Transfer(From: eventLog.Event.From.ConvertToEthereumChecksumAddress(), To: eventLog.Event.To.ConvertToEthereumChecksumAddress(), Value: eventLog.Event.Value, ContractAddress: transactionReceiptVO.TransactionReceipt.To.ConvertToEthereumChecksumAddress())).ToList(),
                Erc721Transfers: erc721TransferLogs?.Select(eventLog => new Erc721Transfer(From: eventLog.Event.From.ConvertToEthereumChecksumAddress(), To: eventLog.Event.To.ConvertToEthereumChecksumAddress(), ContractAddress: transactionReceiptVO.TransactionReceipt.To.ConvertToEthereumChecksumAddress(), TokenId: eventLog.Event.TokenId)).ToList()), cancellationToken);

        }
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