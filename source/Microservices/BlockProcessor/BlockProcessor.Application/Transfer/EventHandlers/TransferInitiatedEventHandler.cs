﻿using BlockProcessor.Domain.Events.Transfer;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using MassTransitManager.Services;

namespace BlockProcessor.Application.Transfer.EventHandlers;

public class TransferInitiatedEventHandler : INotificationHandler<TransferInitiatedEvent>
{
    private readonly ILogger<TransferInitiatedEventHandler> _logger;
    private readonly IMassTransitService _massTransitService;

    public TransferInitiatedEventHandler(ILogger<TransferInitiatedEventHandler> logger, IMassTransitService massTransitService)
    {
        _logger = logger;
        _massTransitService = massTransitService;
    }

    public async Task Handle(TransferInitiatedEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.BlockProcessor, eventName: "Domain Item Created"),
           "BlockProcessor domain event, payment hash: {@Hash} initiated in chain: {@Chain}.",
            notification.Entity.Hash, notification.Entity.Chain), cancellationToken);

        await _massTransitService.PublishAsync<MassTransitManager.Events.Interfaces.ITransferInitiatedEvent>(
                    new MassTransitManager.Events.TransferInitiatedEvent((int)notification.Entity.Chain, notification.Entity.Hash, notification.Entity.From, notification.Entity.To, notification.Entity.Value, 
                        notification.Entity.ConfirmedDatetime, new MassTransitManager.Events.TransferInitiatedEvent.TransferDetails(
                            notification.Entity.Erc20Transfers?.Select(erc20Transfer => new MassTransitManager.Events.Interfaces.ITransferInitiatedEvent.Erc20Transfer(erc20Transfer.From, erc20Transfer.To, erc20Transfer.Value, erc20Transfer.ContractAddress)).ToList(), 
                            notification.Entity.Erc721Transfers?.Select(erc721Transfer => new MassTransitManager.Events.Interfaces.ITransferInitiatedEvent.Erc721Transfer(erc721Transfer.From, erc721Transfer.To, erc721Transfer.TokenId, erc721Transfer.ContractAddress)).ToList()
                            )),
                        cancellationToken);
    }
}
