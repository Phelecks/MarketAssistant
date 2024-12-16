namespace MassTransitManager.Messages.Interfaces;

public interface ICreateBetReferralRewardDocumentMessage
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
    ReferralRewardMetatadaDto Metadata { get; }

    public record ReferralRewardMetatadaDto(ReferralRewardRelatedBetDto relatedBet, ReferralRewardRelatedRewardDto relatedReward);
    public record ReferralRewardRelatedBetDto(Guid id, decimal value, string transactionHash);
    public record ReferralRewardRelatedRewardDto(Guid id, decimal winValue, decimal value, string transactionHash);
    public record NetworkFeeDto(long ExternalTokenId, decimal Value);
}