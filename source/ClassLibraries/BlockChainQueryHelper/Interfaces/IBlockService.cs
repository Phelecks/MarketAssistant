using System.Numerics;
using Nethereum.Web3;

namespace BlockChainQueryHelper.Interfaces;

public interface IBlockService
{
    Task<BigInteger> GetLastBlockAsync(Web3 web3, CancellationToken cancellationToken);
}