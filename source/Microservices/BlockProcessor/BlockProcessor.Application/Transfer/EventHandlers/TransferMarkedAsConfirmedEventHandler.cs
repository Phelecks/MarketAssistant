using BlockProcessor.Domain.Events.Transfer;
using LoggerService.Helpers;
using MassTransitManager.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BlockProcessor.Application.Transfer.EventHandlers;

public class TransferMarkedAsConfirmedEventHandler : INotificationHandler<TransferMarkedAsConfirmedEvent>
{
    private readonly ILogger<TransferMarkedAsConfirmedEventHandler> _logger;
    private readonly IMassTransitService _massTransitService;

    public TransferMarkedAsConfirmedEventHandler(ILogger<TransferMarkedAsConfirmedEventHandler> logger, IMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task Handle(TransferMarkedAsConfirmedEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.BlockProcessor, eventName: "Domain Item Updated"),
           "BlockChain Payment domain event, payment hash: {@Hash} confirmed in chain: {@Chain}.",
            notification.Entity.Hash, notification.Entity.Chain), cancellationToken);

        await _massTransitService.PublishAsync<MassTransitManager.Events.Interfaces.ITransferConfirmedEvent>(
                    new MassTransitManager.Events.TransferConfirmedEvent(notification.Entity.Id, (int)notification.Entity.Chain, notification.Entity.Hash, notification.Entity.From, notification.Entity.To, notification.Entity.Value, 
                        notification.Entity.ConfirmedDatetime, new MassTransitManager.Events.TransferConfirmedEvent.TransferDetails(
                            notification.Entity.Erc20Transfers?.Select(erc20Transfer => new MassTransitManager.Events.Interfaces.ITransferConfirmedEvent.Erc20Transfer(erc20Transfer.From, erc20Transfer.To, erc20Transfer.Value, erc20Transfer.ContractAddress)).ToList(), 
                            notification.Entity.Erc721Transfers?.Select(erc721Transfer => new MassTransitManager.Events.Interfaces.ITransferConfirmedEvent.Erc721Transfer(erc721Transfer.From, erc721Transfer.To, erc721Transfer.TokenId, erc721Transfer.ContractAddress)).ToList()
                            )),
                        cancellationToken);
    }
}
