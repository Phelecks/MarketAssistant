namespace MassTransitManager.Messages.Interfaces;

public interface INotifyBetStatusMessage
{
    string Game { get; }
    long MatchId { get; }
    Guid Id { get; }
    decimal Value { get; }
    long ExternalTokenId { get; }
    string TransactionHash { get; }
    DateTime DateTime { get; }
    OptionDto Option { get; }
    string UserId { get; }
    BetStatus Status { get; }
    DiscordMessageDto? DiscordMessage { get; }

    record OptionDto(long Id, string Title, Uri Thumbnail);
    enum BetStatus { Win, Lose, Draw }
    record DiscordMessageDto(ulong ChannelId, string Game, long MatchId, string UserWalletAddress, string UserWalletAddressUrl, string FormattedValue, string TransactionHash, string TransactionUrl, string OptionTitle, string OptionThumbnailUrl);
}