using BlockProcessor.Application.BlockProgress.Commands.MarkBlockAsProcessed;
using BlockProcessor.Application.BlockProgress.Queries.GetLastProcessedBlock;
using BlockProcessor.Application.Interfaces;
using CacheManager.Interfaces;
using MediatR;
using Nethereum.BlockchainProcessing.ProgressRepositories;
using System.Numerics;

namespace BlockProcessor.Infrastructure.Services;

public class CustomBlockProgressRepository(ISender sender, IDistributedLockService distributedLockService) : ICustomBlockProgressRepository
{
    private readonly ISender _sender = sender;
    private readonly IDistributedLockService _distributedLockService = distributedLockService;

    private Nethereum.Signer.Chain _chain;

    public async Task<IBlockProgressRepository> GetInstanceAsync(Nethereum.Signer.Chain chain)
    {
        _chain = chain;
        return await Task.FromResult(this);
    }

    public async Task<BigInteger?> GetLastBlockNumberProcessedAsync()
    {
        BigInteger lastProcessedBlockNumber = 0;
        await _distributedLockService.RunWithLockAsync(async () =>
        {
            lastProcessedBlockNumber = await _sender.Send(new GetLastProcessedBlockQuery(_chain));
        }, key: $"BlockChainProgressService_{_chain}");
        return lastProcessedBlockNumber;
    }

    public async Task UpsertProgressAsync(BigInteger blockNumber)
    {
        await _sender.Send(new MarkBlockAsProcessedCommand(_chain, blockNumber));
    }
}
