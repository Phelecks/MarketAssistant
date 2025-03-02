using Nethereum.Web3;
using System.Numerics;

namespace BlockChainQueryHelper.Interfaces;

public interface ITransactionService
{
    /// <summary>
    /// Get main token balance of a chain (like matic in polygon)
    /// </summary>
    /// <param name="web3"></param>
    /// <param name="hash"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Nethereum.RPC.Eth.DTOs.Transaction?> GetTransactionByHashAsync(Web3 web3, string hash, CancellationToken cancellationToken);
}