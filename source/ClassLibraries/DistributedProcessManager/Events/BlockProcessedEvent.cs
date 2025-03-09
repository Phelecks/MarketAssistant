using System.Numerics;

namespace DistributedProcessManager.Events;

public class BlockProcessedEvent(Nethereum.Signer.Chain chain, BigInteger blockNumber) : EventArgs
{
    public Nethereum.Signer.Chain Chain { get; } = chain;
    public BigInteger BlockNumber { get; } = blockNumber;
}