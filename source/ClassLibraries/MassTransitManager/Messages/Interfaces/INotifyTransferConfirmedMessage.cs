namespace MassTransitManager.Messages.Interfaces;

public interface INotifyTransferConfirmedMessage : ITransferConfirmedMessage
{
    string UserId { get; }
    DiscordMessage? Discord { get; }

    record DiscordMessage(ulong ChannelId);
}