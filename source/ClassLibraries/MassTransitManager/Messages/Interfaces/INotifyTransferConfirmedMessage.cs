
namespace MassTransitManager.Messages.Interfaces;

public interface INotifyTransferConfirmedMessage : Events.Interfaces.ITransferConfirmedEvent
{
    string UserId { get; }
    DiscordMessage? Discord { get; }

    record DiscordMessage(ulong ChannelId);
}