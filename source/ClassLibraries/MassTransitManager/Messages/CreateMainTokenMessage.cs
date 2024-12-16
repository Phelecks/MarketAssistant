using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateMainTokenMessage : ICreateMainTokenMessage
{
    public CreateMainTokenMessage(long tokenId, string symbol, int chainId, int decimals)
    {
        TokenId = tokenId;
        Symbol = symbol;
        ChainId = chainId;
        Decimals = decimals;
    }

    public long TokenId { get; }
    public string Symbol { get; }
    public int ChainId { get; }
    public int Decimals { get; }
}