using CacheManager.Interfaces;
using DistributedProcessManager.Events;
using Microsoft.Extensions.Caching.Distributed;
using Nethereum.BlockchainProcessing.ProgressRepositories;
using System.Numerics;

namespace DistributedProcessManager.Repositories
{
    public class DistributedBlockChainProgressRepository(IDistributedCache distributedCache, IDistributedLockService distributedLockService) : IBlockProgressRepository
    {
        private readonly IDistributedCache _distributedCache = distributedCache;
        private readonly IDistributedLockService _distributedLockService = distributedLockService;
        private Nethereum.Signer.Chain _chain;
        private string _cacheKey = string.Empty;
        private bool _withCache;

        public BigInteger? LastBlockProcessed { get; private set; }

        public async Task<DistributedBlockChainProgressRepository> GetInstanceAsync(Nethereum.Signer.Chain chain, 
            string cacheKey, BigInteger lastBlockNumber, bool withCache = true)
        {
            LastBlockProcessed = lastBlockNumber;
            _chain = chain;
            _cacheKey = cacheKey;
            _withCache = withCache;
            await WriteBlockNumberAsync(lastBlockNumber);
            return this;
        }

        public async Task<BigInteger?> GetLastBlockNumberProcessedAsync()
        {
            return await ReadBlockNumberAsync();
        }

        public delegate Task AsyncEventHandler<in TEventArgs>(object? sender, TEventArgs e);
        /// <summary>
        /// Fires when a blocknumber update occurred
        /// </summary>
        public event AsyncEventHandler<BlockProcessedEvent>? BlockProcessedEventHandler;
        public async Task UpsertProgressAsync(BigInteger blockNumber)
        {
            if (LastBlockProcessed == blockNumber) return;

            await _distributedLockService.RunWithLockAsync(func: async () => await WriteBlockNumberAsync(blockNumber), 
                key: $"lock_{_cacheKey}_{_chain}");
        }


        private async Task<BigInteger?> ReadBlockNumberAsync()
        {
            BigInteger? result = LastBlockProcessed;
            if(_withCache)
            {
                var cacheResult = await _distributedCache.GetAsync(key: $"{_cacheKey}_{_chain}");
                if (cacheResult is null)
                    return null;

                result = System.Text.Json.JsonSerializer.Deserialize<long>(cacheResult);
            }
            
            return result;
        }

        private async Task WriteBlockNumberAsync(BigInteger blockNumber)
        {
            if (_withCache)
                await _distributedCache.SetAsync(key: $"{_cacheKey}_{_chain}",
                    value: System.Text.Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize((long)blockNumber)),
                    options: new()
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(1)
                    });
            if (BlockProcessedEventHandler is not null)
                await BlockProcessedEventHandler(this, new BlockProcessedEvent(_chain, blockNumber));
            LastBlockProcessed = blockNumber;
        }
    }
}
