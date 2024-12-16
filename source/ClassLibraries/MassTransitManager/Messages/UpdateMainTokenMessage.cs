using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class UpdateMainTokenMessage : IUpdateMainTokenMessage
{
    public UpdateMainTokenMessage(long tokenId, string symbol, int chainId)
    {
        TokenId = tokenId;
        Symbol = symbol;
        ChainId = chainId;
    }

    public long TokenId { get; }
    public string Symbol { get; }
    public int ChainId { get; }
}