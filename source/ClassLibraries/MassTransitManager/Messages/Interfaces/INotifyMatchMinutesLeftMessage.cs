namespace MassTransitManager.Messages.Interfaces;

public interface INotifyMatchMinutesLeftMessage
{
    string Game { get; }
    long MatchId { get; }
    string MatchWalletAddress { get; }
    string MatchWalletAddressUrl { get; }
    double MinutesLeft { get; }
    string ImageUrl { get; }
    DiscordMessageDto? DiscordMessage { get; }

    record DiscordMessageDto(ulong ChannelId);
}