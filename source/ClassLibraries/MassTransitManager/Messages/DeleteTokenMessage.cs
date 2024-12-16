using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class DeleteTokenMessage : IDeleteTokenMessage
{
    public DeleteTokenMessage(long tokenId)
    {
        TokenId = tokenId;
    }

    public long TokenId { get; }
}