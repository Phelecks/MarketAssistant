using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class NotifyMatchOverviewMessage(NotifyMatchOverviewMessage.MatchDto match, string matchWalletAddress, string matchWalletAddressUrl, double minutesLeft, string imageUrl, int bets, string formattedBetValue, INotifyMatchOverviewMessage.DiscordMessageDto? discordMessage) : INotifyMatchOverviewMessage
{
    public string Game { get; } = match.Game;
    public long MatchId { get; } = match.MatchId;
    public string MatchWalletAddress { get; } = matchWalletAddress;
    public string MatchWalletAddressUrl { get; } = matchWalletAddressUrl;
    public double MinutesLeft { get; } = minutesLeft;
    public string ImageUrl { get; } = imageUrl;
    public int Bets { get; } = bets;
    public string FormattedBetValue { get; } = formattedBetValue;
    public INotifyMatchOverviewMessage.DiscordMessageDto? DiscordMessage { get; } = discordMessage;

    public record MatchDto(string Game, long MatchId);
}
