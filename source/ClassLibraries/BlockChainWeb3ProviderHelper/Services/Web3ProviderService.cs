using BlockChainWeb3ProviderHelper.Interfaces;
using DistributedProcessManager.Services;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace BlockChainWeb3ProviderHelper.Services;

public class Web3ProviderService(DistributedNonceService distributedNonceService) : IWeb3ProviderService
{
    private readonly DistributedNonceService _distributedNonceService = distributedNonceService;

    public Web3 CreateWeb3(Chain chain, string rpcUrl, CancellationToken cancellationToken = default)
    {
        var web3 = new Web3(rpcUrl);

        return web3;
    }

    public Web3 CreateWeb3(string privateKey, Chain chain, string rpcUrl, CancellationToken cancellationToken = default)
    {
        var account = new Account(privateKey, chain);
        var web3 = new Web3(account, rpcUrl)
        {
            TransactionManager =
                {
                    UseLegacyAsDefault = true
                }
        };
        account.NonceService = _distributedNonceService.GetInstance(account.Address, web3.Client);

        return web3;
    }
}
