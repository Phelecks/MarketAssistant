namespace MassTransitManager.Messages.Interfaces;

public interface INotifyBetFailedMessage
{
    string Game { get; }
    long MatchId { get; }
    Guid Id { get; }
    decimal Value { get; }
    long ExternalTokenId { get; }
    string TransactionHash { get; }
    DateTime DateTime { get; }
    OptionDto Option { get; }
    string Message { get; }
    string UserId { get; }

    record OptionDto(long Id, string Title, Uri Thumbnail);
}