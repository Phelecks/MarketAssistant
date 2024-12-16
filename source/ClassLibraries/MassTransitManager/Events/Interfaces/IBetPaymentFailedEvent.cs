namespace MassTransitManager.Events.Interfaces;

public interface IBetPaymentFailedEvent
{
    string Game { get; }
    long MatchId { get; }
    string TransactionHash { get; }
}