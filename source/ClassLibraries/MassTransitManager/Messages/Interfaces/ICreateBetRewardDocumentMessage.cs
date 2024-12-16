namespace MassTransitManager.Messages.Interfaces;

public interface ICreateBetRewardDocumentMessage
{
    string Game { get; }
    long MatchId { get; }
    Guid RewardId { get; }
    string TransactionHash { get; }
    string From { get; }
    string To { get; }
    long ExternalTokenId { get; }
    decimal Value { get; }
    DateTime DateTime { get; }
    NetworkFeeDto NetworkFee { get; }
    RewardMetadataDto Metadata { get; }

    public record RewardMetadataDto(RewardRelatedBetDto relatedBet, decimal winValue, List<BoosterDto>? boosters);
    public record RewardRelatedBetDto(Guid id, decimal value, string transactionHash);
    public record BoosterDto(long externalTokenId, int nftId, float rate);
    record NetworkFeeDto(long ExternalTokenId, decimal Value);
}