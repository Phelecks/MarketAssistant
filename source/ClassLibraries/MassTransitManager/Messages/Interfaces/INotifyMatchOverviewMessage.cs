namespace MassTransitManager.Messages.Interfaces;

public interface INotifyMatchOverviewMessage
{
    string Game { get; }
    long MatchId { get; }
    string MatchWalletAddress { get; }
    string MatchWalletAddressUrl { get; }
    double MinutesLeft { get; }
    string ImageUrl { get; }
    int Bets { get; }
    string FormattedBetValue { get; }
    DiscordMessageDto? DiscordMessage { get; }

    record DiscordMessageDto(ulong ChannelId);
}