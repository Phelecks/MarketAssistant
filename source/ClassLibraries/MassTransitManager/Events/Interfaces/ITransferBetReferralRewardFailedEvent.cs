using MassTransit;

namespace MassTransitManager.Events.Interfaces;

public interface ITransferBetReferralRewardFailedEvent
{
    string Game { get; }
    long MatchId { get; }
    Guid RewardId { get; }
    string ErrorMessage { get; }
}