using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface ICreateBetDocumentMessage
{
    string Game { get; }
    long MatchId { get; }
    Guid BetId { get; }
    string TransactionHash { get; }
    string From { get; }
    string To { get; }
    long ExternalTokenId { get; }
    decimal Value { get; }
    DateTime DateTime { get; }
}