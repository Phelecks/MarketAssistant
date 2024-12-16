using MassTransit;

namespace MassTransitManager.Messages.Interfaces;

public interface IBlockChainTransferMessage : CorrelatedBy<Guid>
{
    public int TransactionSequence { get; }
    long ExternalTokenId { get; }
    string To { get; }
    decimal Value { get; }
}