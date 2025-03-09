using BaseApplication.Interfaces;
using BlockChainWeb3ProviderHelper.Interfaces;
using BlockProcessor.Application.BlockProgress.Queries.GetNextProcessingBlock;
using BlockProcessor.Application.Interfaces;
using BlockProcessor.Application.RpcUrl.Queries.GetMinimumBlockOfConfirmation;
using BlockProcessor.Application.RpcUrl.Queries.GetRpcUrl;
using BlockProcessor.Application.RpcUrl.Queries.GetWaitIntervalOfBlockProgress;
using BlockProcessor.Application.Transfer.Commands.InitiateTransfer;
using BlockProcessor.Application.WalletAddress.Queries.GetAllAddresses;
using MediatR;
using Microsoft.Extensions.Logging;
using Nethereum.BlockchainProcessing.BlockProcessing;
using Nethereum.BlockchainProcessing.Orchestrator;
using Nethereum.BlockchainProcessing.ProgressRepositories;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;

namespace BlockProcessor.Infrastructure.Services;

public class BlockProcessorService(ISender sender, ILogger<BlockProcessorService> logger, 
    IWeb3ProviderService web3ProviderService,
    ICustomBlockProgressRepository customBlockProgressRepository, IDateTimeService dateTimeService) : IBlockProcessorService
{
    private readonly ISender _sender = sender;
    private readonly ILogger<BlockProcessorService> _logger = logger;
    private readonly IWeb3ProviderService _web3ProviderService = web3ProviderService;
    private readonly IDateTimeService _dateTimeService = dateTimeService;
    private readonly AddressUtil _addressUtil =  new();
    private readonly ICustomBlockProgressRepository _customBlockProgressRepository = customBlockProgressRepository;
    
    private List<string> _addresses = [];
    public async Task StartAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        var blockProgressRepository = await _customBlockProgressRepository.
            GetInstanceAsync(chain: chain);
        
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

        IBlockchainProcessingOrchestrator orchestrator = new CustomBlockCrawlOrchestrator(ethApi: web3.Eth, blockProcessingSteps: blockProcessingSteps);

        await ExecuteAsync(chain, waitInterval, blockProgressRepository, orchestrator, cancellationToken);
    }

    public Task StopAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RestartAsync(Nethereum.Signer.Chain chain, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task ExecuteAsync(Nethereum.Signer.Chain chain,
        int waitInterval,
        IBlockProgressRepository blockProgressRepository,
        IBlockchainProcessingOrchestrator orchestrator,
        CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(waitInterval, cancellationToken);

            var processingBlockNumber = await _sender.Send(new GetNextProcessingBlockQuery(chain), cancellationToken);
            var progress = await orchestrator.ProcessAsync(processingBlockNumber, processingBlockNumber, cancellationToken, blockProgressRepository);
        }
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
}