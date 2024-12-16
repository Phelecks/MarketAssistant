using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class TransferBetRewardMessage : ITransferBetRewardMessage
{
    public TransferBetRewardMessage(string game, long matchId, Guid rewardId, string from, string to, long externalTokenId, decimal value, ITransferBetRewardMessage.RewardMetadataDto rewardMetadata)
    {
        Game = game;
        MatchId = matchId;
        RewardId = rewardId;
        From = from;
        To = to;
        ExternalTokenId = externalTokenId;
        Value = value;
        RewardMetadata = rewardMetadata;
    }

    public string Game { get; }
    public long MatchId { get; }
    public Guid RewardId { get; }
    public string From { get; }
    public string To { get; }
    public long ExternalTokenId { get; }
    public decimal Value { get; }
    public ITransferBetRewardMessage.RewardMetadataDto RewardMetadata { get; }
}