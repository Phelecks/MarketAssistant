namespace CacheManager.DataTransferObjects;

public class TemporaryBetDto
{
    public string game { get; set; }
    public long matchId { get; set; }
    public long externalTokenId { get; set; }
    public decimal value { get; set; }
    public long optionId { get; set; }
    public string matchWalletAddress { get; set; }
    public string? transactionHash { get; set; }
}
