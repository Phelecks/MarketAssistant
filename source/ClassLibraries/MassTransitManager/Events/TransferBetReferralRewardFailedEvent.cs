using MassTransitManager.Events.Interfaces;

namespace MassTransitManager.Events;

public class TransferBetReferralRewardFailedEvent : ITransferBetReferralRewardFailedEvent
{
    public TransferBetReferralRewardFailedEvent(string game, long matchId, Guid rewardId, string errorMessage)
    {
        Game = game;
        MatchId = matchId;
        RewardId = rewardId;
        ErrorMessage = errorMessage;
    }

    public string Game { get; }
    public long MatchId { get; }
    public Guid RewardId { get; }
    public string ErrorMessage { get; }
}