namespace MassTransitManager.Messages.Interfaces;

public interface INotifyRewardPaidMessage
{
    string Game { get; }
    long MatchId { get; }
    Guid Id { get; }
    decimal Value { get; }
    long ExternalTokenId { get; }
    string TransactionHash { get; }
    DateTime DateTime { get; }
    string UserId { get; }
    RewardType Type { get; }
    DiscordMessageDto? DiscordMessage { get; }

    record DiscordMessageDto(ulong ChannelId, string Game, long MatchId, string UserWalletAddress, string UserWalletAddressUrl, string FormattedValue, string TransactionHash, string TransactionUrl);
    enum RewardType { WinReward, DrawReward, ReferralReward }
}