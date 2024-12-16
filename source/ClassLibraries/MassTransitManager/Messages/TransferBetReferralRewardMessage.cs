using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class TransferBetReferralRewardMessage: ITransferBetReferralRewardMessage
{
    public TransferBetReferralRewardMessage(string game, long matchId, Guid rewardId, string from, string to, long externalTokenId, decimal value, ITransferBetReferralRewardMessage.ReferralRewardMetatadaDto metadata)
    {
        Game = game;
        MatchId = matchId;
        RewardId = rewardId;
        From = from;
        To = to;
        ExternalTokenId = externalTokenId;
        Value = value;
        Metadata = metadata;
    }

    public string Game { get; }
    public long MatchId { get; }
    public Guid RewardId { get; }
    public string From { get; }
    public string To { get; }
    public long ExternalTokenId { get; }
    public decimal Value { get; }
    public ITransferBetReferralRewardMessage.ReferralRewardMetatadaDto Metadata { get; }
}