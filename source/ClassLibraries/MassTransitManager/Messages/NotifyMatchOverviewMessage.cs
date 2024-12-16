using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class NotifyMatchOverviewMessage : INotifyMatchOverviewMessage
{
    public NotifyMatchOverviewMessage(string game, long matchId, string matchWalletAddress, string matchWalletAddressUrl, double minutesLeft, string imageUrl, int bets, string formattedBetValue, INotifyMatchOverviewMessage.DiscordMessageDto? discordMessage)
    {
        Game = game;
        MatchId = matchId;
        MatchWalletAddress = matchWalletAddress;
        MatchWalletAddressUrl = matchWalletAddressUrl;
        MinutesLeft = minutesLeft;
        ImageUrl = imageUrl;
        Bets = bets;
        FormattedBetValue = formattedBetValue;
        DiscordMessage = discordMessage;
    }

    public string Game { get; }
    public long MatchId { get; }
    public string MatchWalletAddress { get; }
    public string MatchWalletAddressUrl { get; }
    public double MinutesLeft { get; }
    public string ImageUrl { get; }
    public int Bets { get; }
    public string FormattedBetValue { get; }
    public INotifyMatchOverviewMessage.DiscordMessageDto? DiscordMessage { get; }
}
