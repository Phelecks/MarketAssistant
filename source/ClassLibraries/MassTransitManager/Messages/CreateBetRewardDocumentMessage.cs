using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateBetRewardDocumentMessage : ICreateBetRewardDocumentMessage
{
    public CreateBetRewardDocumentMessage(string game, long matchId, Guid rewardId, string transactionHash, string from, string to, long externalTokenId, decimal value, DateTime dateTime, ICreateBetRewardDocumentMessage.NetworkFeeDto networkFee, ICreateBetRewardDocumentMessage.RewardMetadataDto metadata)
    {
        Game = game;
        MatchId = matchId;
        RewardId = rewardId;
        TransactionHash = transactionHash;
        From = from;
        To = to;
        ExternalTokenId = externalTokenId;
        Value = value;
        DateTime = dateTime;
        NetworkFee = networkFee;
        Metadata = metadata;
    }

    public string Game { get; }
    public long MatchId { get; }
    public Guid RewardId { get; }
    public string TransactionHash { get; }
    public string From { get; }
    public string To { get; }
    public long ExternalTokenId { get; }
    public decimal Value { get; }
    public DateTime DateTime { get; }
    public ICreateBetRewardDocumentMessage.NetworkFeeDto NetworkFee { get; }
    public ICreateBetRewardDocumentMessage.RewardMetadataDto Metadata { get; }
}