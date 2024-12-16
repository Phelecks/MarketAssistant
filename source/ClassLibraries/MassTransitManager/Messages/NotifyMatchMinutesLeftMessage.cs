using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class NotifyMatchMinutesLeftMessage : INotifyMatchMinutesLeftMessage
{
    public NotifyMatchMinutesLeftMessage(string game, long matchId, string matchWalletAddress, string matchWalletAddressUrl, double minutesLeft, string imageUrl, INotifyMatchMinutesLeftMessage.DiscordMessageDto? discordMessage)
    {
        Game = game;
        MatchId = matchId;
        MatchWalletAddress = matchWalletAddress;
        MatchWalletAddressUrl = matchWalletAddressUrl;
        MinutesLeft = minutesLeft;
        ImageUrl = imageUrl;
        DiscordMessage = discordMessage;
    }

    public string Game { get; }
    public long MatchId { get; }
    public string MatchWalletAddress { get; }
    public string MatchWalletAddressUrl { get; }
    public double MinutesLeft { get; }
    public string ImageUrl { get; }
    public INotifyMatchMinutesLeftMessage.DiscordMessageDto? DiscordMessage { get; }
}
