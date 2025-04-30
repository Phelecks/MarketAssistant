using BaseApplication.Interfaces;
using BlockChainWeb3ProviderHelper.Interfaces;
using LogProcessor.Application.BlockProgress.Commands.MarkBlockAsFailed;
using LogProcessor.Application.BlockProgress.Commands.MarkBlockAsProcessed;
using LogProcessor.Application.BlockProgress.Queries.GetNextProcessingBlock;
using LogProcessor.Application.Interfaces;
using LogProcessor.Application.RpcUrl.Queries.GetRpcUrl;
using LogProcessor.Application.Transfer.Commands.InitiateTransfer;
using LogProcessor.Application.Token.Queries.GetAllTokens;
using ExecutorManager.Helpers;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Polly;
using Nethereum.BlockchainProcessing.Processor;
using Nethereum.Web3;
using System.Numerics;
using System.Collections.Concurrent;

namespace LogProcessor.Infrastructure.Services;

public class LogProcessorService(ISender sender, ILogger<LogProcessorService> logger, 
    IWeb3ProviderService web3ProviderService, [FromKeyedServices(PipelineHelper.RetryEverythingFiveTimes)] ResiliencePipeline pollyPipeline,
    IDateTimeService dateTimeService) : ILogProcessorService
{
    private readonly ISender _sender = sender;
    private readonly ILogger<LogProcessorService> _logger = logger;
    private readonly IWeb3ProviderService _web3ProviderService = web3ProviderService;
    private readonly IDateTimeService _dateTimeService = dateTimeService;
    private readonly ResiliencePipeline _pollyPipeline = pollyPipeline;
    
    public async Task StartAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        var rpcUrl = await _sender.Send(new GetRpcUrlQuery(chain), cancellationToken);
        var web3 = _web3ProviderService.CreateWeb3(rpcUrl.Uri.ToString());
         
        await ExecuteAsync(web3, chain, rpcUrl.WaitInterval, rpcUrl.MaxDegreeOfParallelism, cancellationToken);
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
        int waitInterval, int maxDegreeOfParallelism,
        CancellationToken cancellationToken)
    {
        IEnumerable<ProcessorHandler<FilterLog>> logProcessorHandlers =
        [
            //erc721EventLogProcessorHandler
            new EventLogProcessorHandler<Nethereum.Contracts.Standards.ERC721.ContractDefinition.TransferEventDTO>(async (eventLog) =>
                await ProcessErc721LogAsync(web3, chain, eventLog, cancellationToken)),
            //erc20EventLogProcessorHandler
            new EventLogProcessorHandler<Nethereum.Contracts.Standards.ERC20.ContractDefinition.TransferEventDTO>(async (eventLog) =>
                await ProcessErc20LogAsync(web3, chain, eventLog, cancellationToken))
        ];

        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(waitInterval, cancellationToken);

            var tokens = await _sender.Send(new GetAllTokensQuery(chain), cancellationToken);

            var filterInput = new NewFilterInput
            {
                Address = [.. tokens.Select(s => s.ContractAddress)]
            };

            var processingBlocks = await _sender.Send(new GetNextProcessingBlockQuery(chain), cancellationToken);
            await ParallelProcessBlocksAsync(chain, web3, logProcessorHandlers, filterInput, processingBlocks, maxDegreeOfParallelism, cancellationToken);
        }
    }

    async ValueTask ParallelProcessBlocksAsync(Nethereum.Signer.Chain chain, Web3 web3, 
        IEnumerable<ProcessorHandler<FilterLog>> logProcessorHandlers, NewFilterInput filterInput, 
        BigInteger[] processingBlocks, int maxDegreeOfParallelism, 
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
                        await ProcessBlockAsync(web3, blockNumber, logProcessorHandlers, filterInput, cancellationToken);
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
                eventId: EventTool.GetEventInformation(eventType: EventType.LogProcessorException, eventName: "LogProcessorException"),
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
    
    async ValueTask ProcessBlockAsync(Web3 web3, BigInteger blockNumber, IEnumerable<ProcessorHandler<FilterLog>> logProcessorHandlers, NewFilterInput filterInput, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var processor = web3.Processing.Logs.CreateProcessor(logProcessors: logProcessorHandlers, filter: filterInput, minimumBlockConfirmations: 10);
        
        await _pollyPipeline.ExecuteAsync(async ct => await processor.ExecuteAsync(
            toBlockNumber: blockNumber,
            cancellationToken: cancellationToken,
            startAtBlockNumberIfNotProcessed: blockNumber), cancellationToken);
    }

    private async Task ProcessErc20LogAsync(Web3 web3, Nethereum.Signer.Chain chain, EventLog<Nethereum.Contracts.Standards.ERC20.ContractDefinition.TransferEventDTO> eventLog, CancellationToken cancellationToken)
    {
        var blockWithTransactionHashes = await _pollyPipeline.ExecuteAsync(async ct => await web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(eventLog.Log.BlockNumber), cancellationToken);

        var transactionReceipt = await _pollyPipeline.ExecuteAsync(async ct => await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(eventLog.Log.TransactionHash), cancellationToken);
    
        if(transactionReceipt.Succeeded())
        {
            var erc20TransferLogs = transactionReceipt.Logs.DecodeAllEvents<Nethereum.Contracts.Standards.ERC20.ContractDefinition.TransferEventDTO>();
            var erc721TransferLogs = transactionReceipt.Logs.DecodeAllEvents<Nethereum.Contracts.Standards.ERC721.ContractDefinition.TransferEventDTO>();
                        
            await _sender.Send(new InitiateTransferCommand(
                Hash: transactionReceipt.TransactionHash, From: transactionReceipt.From.ConvertToEthereumChecksumAddress(), To: transactionReceipt.To.ConvertToEthereumChecksumAddress(), 
                Value: BigInteger.Zero, GasUsed: transactionReceipt.GasUsed.Value, EffectiveGasPrice: transactionReceipt.EffectiveGasPrice.Value,
                CumulativeGasUsed: transactionReceipt.CumulativeGasUsed.Value, BlockNumber: transactionReceipt.BlockNumber.Value, ConfirmedDatetime: _dateTimeService.ConvertFromUnixTimestamp(blockWithTransactionHashes.Timestamp.Value), Chain: chain, 
                Erc20Transfers: erc20TransferLogs?.Select(eventLog => new Erc20Transfer(From: eventLog.Event.From.ConvertToEthereumChecksumAddress(), To: eventLog.Event.To.ConvertToEthereumChecksumAddress(), Value: eventLog.Event.Value, ContractAddress: eventLog.Log.Address.ConvertToEthereumChecksumAddress())).ToList(),
                Erc721Transfers: erc721TransferLogs?.Select(eventLog => new Erc721Transfer(From: eventLog.Event.From.ConvertToEthereumChecksumAddress(), To: eventLog.Event.To.ConvertToEthereumChecksumAddress(), ContractAddress: eventLog.Log.Address.ConvertToEthereumChecksumAddress(), TokenId: eventLog.Event.TokenId)).ToList()), cancellationToken);
        }
    }

    private async Task ProcessErc721LogAsync(Web3 web3, Nethereum.Signer.Chain chain, EventLog<Nethereum.Contracts.Standards.ERC721.ContractDefinition.TransferEventDTO> eventLog, CancellationToken cancellationToken)
    {
        var blockWithTransactionHashes = await _pollyPipeline.ExecuteAsync(async ct => await web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(eventLog.Log.BlockNumber), cancellationToken);

        var transactionReceipt = await _pollyPipeline.ExecuteAsync(async ct => await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(eventLog.Log.TransactionHash), cancellationToken);
    
        if(transactionReceipt.Succeeded())
        {
            var erc20TransferLogs = transactionReceipt.Logs.DecodeAllEvents<Nethereum.Contracts.Standards.ERC20.ContractDefinition.TransferEventDTO>();
            var erc721TransferLogs = transactionReceipt.Logs.DecodeAllEvents<Nethereum.Contracts.Standards.ERC721.ContractDefinition.TransferEventDTO>();
                        
            await _sender.Send(new InitiateTransferCommand(
                Hash: transactionReceipt.TransactionHash, From: transactionReceipt.From.ConvertToEthereumChecksumAddress(), To: transactionReceipt.To.ConvertToEthereumChecksumAddress(), 
                Value: BigInteger.Zero, GasUsed: transactionReceipt.GasUsed.Value, EffectiveGasPrice: transactionReceipt.EffectiveGasPrice.Value,
                CumulativeGasUsed: transactionReceipt.CumulativeGasUsed.Value, BlockNumber: transactionReceipt.BlockNumber.Value, ConfirmedDatetime: _dateTimeService.ConvertFromUnixTimestamp(blockWithTransactionHashes.Timestamp.Value), Chain: chain, 
                Erc20Transfers: erc20TransferLogs?.Select(eventLog => new Erc20Transfer(From: eventLog.Event.From.ConvertToEthereumChecksumAddress(), To: eventLog.Event.To.ConvertToEthereumChecksumAddress(), Value: eventLog.Event.Value, ContractAddress: eventLog.Log.Address.ConvertToEthereumChecksumAddress())).ToList(),
                Erc721Transfers: erc721TransferLogs?.Select(eventLog => new Erc721Transfer(From: eventLog.Event.From.ConvertToEthereumChecksumAddress(), To: eventLog.Event.To.ConvertToEthereumChecksumAddress(), ContractAddress: eventLog.Log.Address.ConvertToEthereumChecksumAddress(), TokenId: eventLog.Event.TokenId)).ToList()), cancellationToken);
        }
    }

    record BlockStatus(bool Processed, BigInteger BlockNumber);
}