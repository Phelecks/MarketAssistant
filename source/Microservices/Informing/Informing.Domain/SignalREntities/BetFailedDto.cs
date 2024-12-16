namespace Informing.Domain.SignalREntities;

public class BetFailedDto
{
    public BetFailedDto(string game, long matchId, Guid id, decimal value, long externalTokenId, string transactionHash, DateTime dateTime, OptionDto option, string message)
    {
        this.game = game;
        this.matchId = matchId;
        this.id = id;
        this.value = value;
        this.externalTokenId = externalTokenId;
        this.transactionHash = transactionHash;
        this.dateTime = dateTime;
        this.option = option;
        this.message = message;
    }

    public string game { get; }
    public long matchId { get; }
    public Guid id { get; }
    public decimal value { get; }
    public long externalTokenId { get; }
    public string transactionHash { get; }
    public DateTime dateTime { get; }
    public OptionDto option { get; }
    public string message { get; }


    public class OptionDto
    {
        public OptionDto(long id, string title, Uri thumbnail)
        {
            this.id = id;
            this.title = title;
            this.thumbnail = thumbnail;
        }

        public long id { get; }
        public string title { get; }
        public Uri thumbnail { get; }
    }
}