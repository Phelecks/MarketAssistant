using BlockProcessor.Domain.Events.Transfer;
using LoggerService.Helpers;
using MassTransitManager.Helpers;
using MassTransitManager.Messages;
using MassTransitManager.Messages.Interfaces;
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

        await _massTransitService.SendAsync<ITransferConfirmedMessage>(
                    new TransferConfirmedMessage(
                        correlationId: notification.Entity.Id, 
                        transfer: new TransferConfirmedMessage.Transfer(
                            Chain: notification.Entity.Chain, 
                            Hash: notification.Entity.Hash, 
                            From: notification.Entity.From, 
                            To: notification.Entity.To, 
                            Value: notification.Entity.Value, 
                            DateTime: notification.Entity.ConfirmedDatetime,
                            Erc20Transfers: notification.Entity.Erc20Transfers?.Select(erc20Transfer => new ITransferConfirmedMessage.Erc20Transfer(erc20Transfer.From, erc20Transfer.To, erc20Transfer.Value, erc20Transfer.ContractAddress)).ToList(), 
                            Erc721Transfers: notification.Entity.Erc721Transfers?.Select(erc721Transfer => new ITransferConfirmedMessage.Erc721Transfer(erc721Transfer.From, erc721Transfer.To, erc721Transfer.TokenId, erc721Transfer.ContractAddress)).ToList()
                            )),
                        Queues.TransferConfirmedMessageQueueName,
                        cancellationToken);
    }
}
