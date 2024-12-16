namespace MassTransitManager.Events.Interfaces;

public interface ITransferBetRewardFailedEvent
{
    string Game { get; }
    long MatchId { get; }
    Guid RewardId { get; }
    string ErrorMessage { get; }
}