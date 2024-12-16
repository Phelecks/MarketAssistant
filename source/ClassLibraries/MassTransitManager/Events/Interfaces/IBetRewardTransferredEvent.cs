namespace MassTransitManager.Events.Interfaces;

public interface IBetRewardTransferredEvent
{
    string Game { get; }
    long MatchId { get; }
    Guid RewardId { get; }
    string TransactionHash { get; }
    DateTime DateTime { get; }
    NetworkFeeDto NetworkFee { get; }

    record NetworkFeeDto(long ExternalTokenId, decimal Value);
}