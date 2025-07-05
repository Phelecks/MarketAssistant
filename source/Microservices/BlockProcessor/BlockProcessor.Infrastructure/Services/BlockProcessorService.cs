using System.Collections.Concurrent;
using System.Numerics;
using BaseApplication.Interfaces;
using BlockChainWeb3ProviderHelper.Interfaces;
using BlockProcessor.Application.BlockProgress.Commands.MarkBlockAsFailed;
using BlockProcessor.Application.BlockProgress.Commands.MarkBlockAsProcessed;
using BlockProcessor.Application.BlockProgress.Queries.GetNextProcessingBlock;
using BlockProcessor.Application.Interfaces;
using BlockProcessor.Application.RpcUrl.Queries.GetRpcUrl;
using BlockProcessor.Application.Transfer.Commands.InitiateTransfer;
using BlockProcessor.Application.WalletAddress.Queries.GetAllAddresses;
using ExecutorManager.Helpers;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using Polly;

namespace BlockProcessor.Infrastructure.Services;

public class BlockProcessorService(ISender sender, ILogger<BlockProcessorService> logger, 
    IWeb3ProviderService web3ProviderService, [FromKeyedServices(PipelineHelper.RetryEverythingFiveTimes)] ResiliencePipeline pollyPipeline,
    IDateTimeService dateTimeService) : IBlockProcessorService
{
    private readonly ISender _sender = sender;
    private readonly ILogger<BlockProcessorService> _logger = logger;
    private readonly IWeb3ProviderService _web3ProviderService = web3ProviderService;
    private readonly IDateTimeService _dateTimeService = dateTimeService;
    private readonly ResiliencePipeline _pollyPipeline = pollyPipeline;
    
    private List<string> _addresses = [];

    public async Task StartAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        var rpcUrl = await _sender.Send(new GetRpcUrlQuery(chain), cancellationToken);
        var web3 = _web3ProviderService.CreateWeb3(rpcUrl.Uri.ToString());
        
        
        await ExecuteAsync(web3, chain, rpcUrl.WaitInterval, rpcUrl.MaxDegreeOfParallelism, (uint)rpcUrl.BlockOfConfirmation, cancellationToken);
    }

    public Task StopAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RestartAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task ExecuteAsync(Web3 web3, Nethereum.Signer.Chain chain,
        int waitInterval, int maxDegreeOfParallelism, uint minimumBlockConfirmations,
        CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(waitInterval, cancellationToken);

            _addresses = await _sender.Send(new GetAllAddressesQuery(), cancellationToken);

            var processingBlocks = await _sender.Send(new GetNextProcessingBlockQuery(chain), cancellationToken);
            await ParallelProcessBlocksAsync(chain, web3, processingBlocks, maxDegreeOfParallelism, minimumBlockConfirmations, cancellationToken);
        }
    }

    async ValueTask ParallelProcessBlocksAsync(Nethereum.Signer.Chain chain, Web3 web3,
        BigInteger[] processingBlocks, int maxDegreeOfParallelism, uint minimumBlockConfirmations,
        CancellationToken cancellationToken)
    {
        ConcurrentBag<BlockStatus> blockStatuses = [];
        var numberOfProcessors = Environment.ProcessorCount;

        void TryAddBlockStatus(BlockStatus blockStatus)
        {
            if(blockStatuses.Any(item => item.BlockNumber == blockStatus.BlockNumber)) return;
            blockStatuses.Add(blockStatus);
        }

        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = numberOfProcessors < maxDegreeOfParallelism ? numberOfProcessors : maxDegreeOfParallelism,
            CancellationToken = cancellationToken
        };

        try
        {
            await Parallel.ForEachAsync(
                source: processingBlocks,
                parallelOptions: parallelOptions,
                body: async (blockNumber, CancellationToken) =>
                {
                    try
                    {
                        await ProcessBlockAsync(web3, chain, blockNumber, minimumBlockConfirmations, cancellationToken);
                        TryAddBlockStatus(new BlockStatus(true, blockNumber));
                    }
                    catch
                    {
                        TryAddBlockStatus(new BlockStatus(false, blockNumber));
                    }
                });
        }
        catch (Exception exception)
        {
            foreach(var processingBlock in processingBlocks.Where(processingBlock => !blockStatuses.Any(blockStatus => processingBlock == blockStatus.BlockNumber)))
                TryAddBlockStatus(new BlockStatus(false, processingBlock));
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.Exception, eventName: "BlockProcessorException"),
                exception: exception, message: exception.Message, cancellationToken), cancellationToken);
        }
        finally
        {
            foreach(var blockStatus in blockStatuses.Where(exp => exp.Processed))
                await _sender.Send(new MarkBlockAsProcessedCommand(chain, (long)blockStatus.BlockNumber), cancellationToken);
            foreach(var blockStatus in blockStatuses.Where(exp => !exp.Processed))
                await _sender.Send(new MarkBlockAsFailedCommand(chain, (long)blockStatus.BlockNumber), cancellationToken);
            foreach(var processingBlock in processingBlocks.Where(processingBlock => !blockStatuses.Any(blockStatus => blockStatus.BlockNumber == processingBlock)))
                await _sender.Send(new MarkBlockAsFailedCommand(chain, (long)processingBlock), cancellationToken);
        }
    }

    async ValueTask ProcessBlockAsync(Web3 web3, Nethereum.Signer.Chain chain, BigInteger blockNumber, uint minimumBlockConfirmations, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var processor = web3.Processing.Blocks.CreateBlockProcessor(stepsConfiguration: steps => 
        {
            steps.TransactionStep.SetMatchCriteria(criteria =>
                _addresses is not null &&
                _addresses.Any(address => 
                    // criteria for transaction with source and destination matches with any address in the list
                    criteria.Transaction.IsFrom(address) || criteria.Transaction.IsTo(address)));
            steps.FilterLogStep.SetMatchCriteria(criteria =>
                criteria.IsLogForEvent<Nethereum.Contracts.Standards.ERC20.ContractDefinition.TransferEventDTO>() &&
                _addresses is not null &&
                _addresses.Any(address => criteria.IsTo(address) || criteria.Transaction.IsFrom(address)));
            steps.FilterLogStep.SetMatchCriteria(criteria =>
                criteria.IsLogForEvent<Nethereum.Contracts.Standards.ERC721.ContractDefinition.TransferEventDTO>() &&
                _addresses is not null &&
                _addresses.Any(address => criteria.IsTo(address) || criteria.Transaction.IsFrom(address)));
            steps.TransactionReceiptStep.AddProcessorHandler(async (transactionReceipt) => await ProcessTransactionAsync(chain, transactionReceipt, cancellationToken));
        }, minimumBlockConfirmations: minimumBlockConfirmations, _logger);
        
        await _pollyPipeline.ExecuteAsync(async ct => await processor.ExecuteAsync(
            toBlockNumber: blockNumber,
            cancellationToken: cancellationToken,
            startAtBlockNumberIfNotProcessed: blockNumber), cancellationToken);
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

    record BlockStatus(bool Processed, BigInteger BlockNumber);
}