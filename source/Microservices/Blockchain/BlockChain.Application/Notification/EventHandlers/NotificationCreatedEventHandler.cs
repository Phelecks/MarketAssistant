using BlockChain.Domain.Events.Notification;
using LoggerService.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using MassTransitManager.Services;
using MassTransitManager.Helpers;
using BlockChain.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlockChain.Application.Notification.EventHandlers;

public class NotificationCreatedEventHandler(ILogger<NotificationCreatedEventHandler> logger, IMassTransitService massTransitService, IApplicationDbContext context) : INotificationHandler<NotificationCreatedEvent>
{
    private readonly ILogger<NotificationCreatedEventHandler> _logger = logger;
    private readonly IMassTransitService _massTransitService = massTransitService;
    private readonly IApplicationDbContext _context = context;

    public async Task Handle(NotificationCreatedEvent notification, CancellationToken cancellationToken)
    {
        _ = Task.Run(() => _logger.LogInformation(
           eventId: EventTool.GetEventInformation(eventType: EventType.BlockChain, eventName: "Domain Item Created"),
           "BlockChain domain event, notification for hash: {@Hash} created in chain: {@Chain}.",
            notification.WalletAddress, notification.Chain), cancellationToken);

        var notifications = await _context.Notifications.Where(exp => exp.CustomerWalletAddress.Equals(notification.WalletAddress)).ToListAsync(cancellationToken);
        
        await _massTransitService.SendAsync<MassTransitManager.Messages.Interfaces.INotifyTransferConfirmedMessage>(
                    new MassTransitManager.Messages.NotifyTransferConfirmedMessage(
                        correlationId: notification.CorrelationId, 
                        userId: notification.WalletAddress, 
                        discord: GetDiscordMessage(notifications),
                        transfer: new MassTransitManager.Messages.NotifyTransferConfirmedMessage.Transfer(
                            Chain: notification.Chain, 
                            Hash: notification.Hash, 
                            From: notification.From, 
                            To: notification.To, 
                            Value: notification.Value, 
                            DateTime: notification.DateTime,
                            Erc20Transfers: notification.Erc20Transfers?.Select(erc20Transfer => new MassTransitManager.Events.Interfaces.ITransferConfirmedEvent.Erc20Transfer(erc20Transfer.From, erc20Transfer.To, erc20Transfer.Value, erc20Transfer.ContractAddress)).ToList(), 
                            Erc721Transfers: notification.Erc721Transfers?.Select(erc721Transfer => new MassTransitManager.Events.Interfaces.ITransferConfirmedEvent.Erc721Transfer(erc721Transfer.From, erc721Transfer.To, erc721Transfer.TokenId, erc721Transfer.ContractAddress)).ToList()
                            )),
                        Queues.NotifyTransferConfirmedMessageQueueName,                            
                        cancellationToken);
    }

    private static MassTransitManager.Messages.Interfaces.INotifyTransferConfirmedMessage.DiscordMessage? GetDiscordMessage(List<Domain.Entities.Notification>? notifications)
    {
        var discordNotification = notifications?.FirstOrDefault(exp => exp.Type == Domain.Entities.Notification.NotificationType.Discord);
        if(discordNotification is null) return null;

        if(ulong.TryParse(discordNotification.Identifier, out ulong discordIdentifier))
            return new MassTransitManager.Messages.Interfaces.INotifyTransferConfirmedMessage.DiscordMessage(discordIdentifier);
        
        return null;
    }
}
