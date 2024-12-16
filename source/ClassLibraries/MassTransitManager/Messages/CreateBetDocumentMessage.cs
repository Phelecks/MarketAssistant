using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateBetDocumentMessage : ICreateBetDocumentMessage
{
    public CreateBetDocumentMessage(string game, long matchId, Guid betId, string transactionHash, string from, string to, decimal value, long externalTokenId, DateTime datetime)
    {
        Game = game;
        MatchId = matchId;
        BetId = betId;
        TransactionHash = transactionHash;
        From = from;
        To = to;
        ExternalTokenId = externalTokenId;
        Value = value;
        DateTime = datetime;
    }

    public string Game { get; }
    public long MatchId { get; }
    public Guid BetId { get; }
    public string TransactionHash { get; }
    public string From { get; }
    public string To { get; }
    public long ExternalTokenId { get; }
    public decimal Value { get; }
    public DateTime DateTime { get; }
}