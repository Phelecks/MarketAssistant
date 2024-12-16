namespace MassTransitManager.Messages.Interfaces;

public interface ITransferBetRewardMessage
{
    string Game { get; }
    long MatchId { get; }
    Guid RewardId { get; }
    string From { get; }
    string To { get; }
    long ExternalTokenId { get; }
    decimal Value { get; }
    RewardMetadataDto RewardMetadata { get; }

    public record RewardMetadataDto(RewardRelatedBetDto relatedBet, decimal winValue, List<BoosterDto>? boosters);
    public record RewardRelatedBetDto(Guid id, decimal value, string transactionHash);
    public record BoosterDto(long externalTokenId, int nftId, float rate);
}