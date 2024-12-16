namespace MassTransitManager.Events.Interfaces;

public interface IBetPaymentConfirmedEvent
{
    string Game { get; }
    long MatchId { get; }
    string TransactionHash { get; }
}