using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class BetRewardTransferredEvent : IBetRewardTransferredEvent
{
    public BetRewardTransferredEvent(string game, long matchId, Guid rewardId, string transactionHash, DateTime dateTime, IBetRewardTransferredEvent.NetworkFeeDto networkFee)
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
    public string TransactionHash { get; }
    public DateTime DateTime { get; }
    public IBetRewardTransferredEvent.NetworkFeeDto NetworkFee { get; }
}