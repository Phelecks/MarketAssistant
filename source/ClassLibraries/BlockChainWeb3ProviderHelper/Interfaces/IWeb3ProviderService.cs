using Nethereum.Web3;

namespace BlockChainWeb3ProviderHelper.Interfaces;

public interface IWeb3ProviderService
{
    /// <summary>
    /// Use it for inquiry balance and not for ransactions
    /// </summary>
    /// <param name="chain"></param>
    /// <returns></returns>
    Web3 CreateWeb3(string rpcUrl);

    /// <summary>
    /// Use it for transactions
    /// </summary>
    /// <param name="privateKey"></param>
    /// <param name="chain"></param>
    /// <returns></returns>
    Web3 CreateWeb3(string privateKey, Nethereum.Signer.Chain chain, string rpcUrl);
}