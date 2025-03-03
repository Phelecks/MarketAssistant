using System.Numerics;
using BlockChainQueryHelper.Interfaces;
using Nethereum.Web3;

namespace BlockChainQueryHelper.Services;

public class BlockService : IBlockService
{
    public async Task<BigInteger> GetLastBlockAsync(Web3 web3, CancellationToken cancellationToken)
    {
        return await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
    }
}