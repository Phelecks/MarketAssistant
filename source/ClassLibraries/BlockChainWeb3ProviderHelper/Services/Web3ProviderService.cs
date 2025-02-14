using BlockChainWeb3ProviderHelper.Interfaces;
using CacheManager.Interfaces;
using DistributedProcessManager.Services;
using Microsoft.Extensions.Configuration;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace BlockChainWeb3ProviderHelper.Services;

public class Web3ProviderService : IWeb3ProviderService
{
    private readonly IDistributedLockService _distributedLockService;
    private readonly IConfiguration _configuration;

    public Web3ProviderService(IDistributedLockService distributedLockService, IConfiguration configuration)
    {
        _distributedLockService = distributedLockService;
        _configuration = configuration;
    }

    public Web3 CreateWeb3(Chain chain, CancellationToken cancellationToken = default)
    {
        var rpcUrl = GetRpcUrl(chain, cancellationToken);
        var web3 = new Web3(rpcUrl);

        return web3;
    }

    public Web3 CreateWeb3(string privateKey, Chain chain, CancellationToken cancellationToken = default)
    {
        var rpcUrl = GetRpcUrl(chain, cancellationToken);

        var account = new Account(privateKey, chain);
        var web3 = new Web3(account, rpcUrl)
        {
            TransactionManager =
                {
                    UseLegacyAsDefault = true
                }
        };
        //account.NonceService = new InMemoryNonceService(account.Address, web3.Client);
        account.NonceService = new DistributedNonceService(account.Address, web3.Client, _distributedLockService);

        return web3;
    }

    string GetRpcUrl(Chain chain, CancellationToken cancellationToken)
    {
        string? rpcUrl;
        switch (chain)
        {
            case Chain.Polygon:
                rpcUrl = _configuration.GetValue("", "");
                break;
            case Chain.MainNet:
                rpcUrl = _configuration.GetValue("", "");
                break;
            case Chain.ClassicTestNet:
                rpcUrl = _configuration.GetValue("", "");
                break;
            case Chain.Arbitrum:
                rpcUrl = _configuration.GetValue("", ""); 
                break;
            case Chain.Optimism:
                rpcUrl = _configuration.GetValue("", "");
                break;
            case Chain.Astar:
                rpcUrl = _configuration.GetValue("", "");
                break;
            case Chain.Binance:
                rpcUrl = _configuration.GetValue("", "");
                break;
            case Chain.Mantle:
                rpcUrl = _configuration.GetValue("", "");
                break;
            case Chain.Avalanche:
                rpcUrl = _configuration.GetValue("", "");
                break;
            default:
                rpcUrl = null;
                break;
        }

        if (rpcUrl is null)
            throw new Exception($"RPC Url for {chain} not found.");

        return rpcUrl;
    }
}
