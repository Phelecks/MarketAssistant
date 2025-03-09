using Nethereum.BlockchainProcessing.ProgressRepositories;

namespace BlockProcessor.Application.Interfaces;

public interface ICustomBlockProgressRepository : IBlockProgressRepository
{
    Task<IBlockProgressRepository> GetInstanceAsync(Nethereum.Signer.Chain chain);
}
