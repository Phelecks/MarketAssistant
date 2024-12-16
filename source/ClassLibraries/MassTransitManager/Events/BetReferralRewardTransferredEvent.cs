using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class BetReferralRewardTransferredEvent : IBetReferralRewardTransferredEvent
{
    public BetReferralRewardTransferredEvent(string game, long matchId, Guid rewardId, string transactionHash, DateTime dateTime, IBetReferralRewardTransferredEvent.NetworkFeeDto networkFee)
    {
        TransactionHash = transactionHash;
        DateTime = dateTime;
        NetworkFee = networkFee;
        Game = game;
        MatchId = matchId;
        RewardId = rewardId;
    }

    public string Game { get; }
    public long MatchId { get; }
    public Guid RewardId { get; }
    public Guid CorrelationId { get; }
    public string TransactionHash { get; }
    public DateTime DateTime { get; }
    public IBetReferralRewardTransferredEvent.NetworkFeeDto NetworkFee { get; }
}