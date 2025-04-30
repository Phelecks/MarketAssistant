using BlockChainQueryHelper.Interfaces;
using BlockChainWeb3ProviderHelper.Interfaces;
using LogProcessor.Domain.Events.BlockProgress;
using LogProcessor.Domain.Events.Transfer;
using LoggerService.Helpers;
using LogProcessor.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LogProcessor.Application.BlockProgress.EventHandlers;

public class BlockProcessedEventHandler(IApplicationDbContext context, ILogger<BlockProcessedEventHandler> logger, IWeb3ProviderService web3ProviderService, ITransactionService transactionService) : INotificationHandler<BlockProcessedEvent>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ILogger<BlockProcessedEventHandler> _logger = logger;
    private readonly IWeb3ProviderService _web3ProviderService = web3ProviderService;
    private readonly ITransactionService _transactionService = transactionService;

    public async Task Handle(BlockProcessedEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.BlockProcessor, eventName: "Domain Item Updated"),
           "BlockProcessor domain event, {@Object} updated with {@Item}.",
           notification.GetType().Name, notification.Entity), cancellationToken);

        var rpcUrl = await _context.RpcUrls.SingleAsync(exp => exp.Chain == notification.Entity.Chain, cancellationToken);
        var blockConfirmations = rpcUrl.BlockOfConfirmation;

        //This is the block that all related transactions are passed the minimum block confirmation threshold
        var confirmedBlockNumber = notification.Entity.BlockNumber - blockConfirmations;

        var blockTransfers = await _context.Transfers.Include(inc => inc.Erc20Transfers).Include(inc => inc.Erc721Transfers).Where(exp => exp.BlockNumber == confirmedBlockNumber).ToListAsync(cancellationToken);

        foreach(var blockTransfer in blockTransfers)
        {
            var blockTransferRpcUrl = await _context.RpcUrls.SingleAsync(exp => exp.Chain == notification.Entity.Chain, cancellationToken);
            var web3 = _web3ProviderService.CreateWeb3(blockTransferRpcUrl.Uri.ToString());
            var blockChainTransaction = await _transactionService.GetTransactionByHashAsync(web3, blockTransfer.Hash, cancellationToken);
            if(blockChainTransaction is null)
            {
                blockTransfer.State = Domain.Entities.Transfer.TransferState.Failed;
                blockTransfer.AddDomainEvent(new TransferMarkedAsFailedEvent(blockTransfer, "Transaction not found or reveresed."));
            }
            else
            {
                blockTransfer.State = Domain.Entities.Transfer.TransferState.Confirmed;
                blockTransfer.AddDomainEvent(new TransferMarkedAsConfirmedEvent(blockTransfer));
            }
        }
    }
}
