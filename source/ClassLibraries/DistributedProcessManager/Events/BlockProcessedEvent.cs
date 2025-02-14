using System.Numerics;

namespace DistributedProcessManager.Events;

public class BlockProcessedEvent : EventArgs
{
    public BlockProcessedEvent(Nethereum.Signer.Chain chain, BigInteger blockNumber)
    {
        Chain = chain;
        BlockNumber = blockNumber;
    }

    public Nethereum.Signer.Chain Chain { get; }
    public BigInteger BlockNumber { get; }
}