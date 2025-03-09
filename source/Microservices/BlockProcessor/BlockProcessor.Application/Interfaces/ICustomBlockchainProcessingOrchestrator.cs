using System.Numerics;

namespace BlockProcessor.Application.Interfaces;

public interface ICustomBlockchainProcessingOrchestrator
{
    Task ProcessAsync(BigInteger blockNumber, CancellationToken cancellationToken);  
}