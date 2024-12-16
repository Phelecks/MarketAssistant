using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class BlockChainTransferMessage : IBlockChainTransferMessage
{
    public Guid CorrelationId { get; }
    public int TransactionSequence { get; }
    public long ExternalTokenId { get; }
    public string To { get; }
    public decimal Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="clientId"></param>
    public BlockChainTransferMessage(Guid correlationId, int transactionSequence, long externalTokenId, string to, decimal value)
    {
        ExternalTokenId = externalTokenId;
        To = to;
        Value = value;
        CorrelationId = correlationId;
        TransactionSequence = transactionSequence;
    }
}