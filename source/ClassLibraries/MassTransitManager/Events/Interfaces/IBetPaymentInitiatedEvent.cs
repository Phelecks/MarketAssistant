namespace MassTransitManager.Events.Interfaces;

public interface IBetPaymentInitiatedEvent
{
    string Game { get; }
    long MatchId { get; }
    string TransactionHash { get; }
    string From { get; }
    string To { get; }
    long ExternalTokenId { get; } 
    decimal Value { get; }
    DateTime DateTime { get; }
}