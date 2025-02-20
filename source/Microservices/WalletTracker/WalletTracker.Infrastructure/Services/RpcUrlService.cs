using Nethereum.Signer;
using System.Collections.Concurrent;
using WalletTracker.Application.Interfaces;

namespace WalletTracker.Infrastructure.Services;

public class RpcUrlService : IRpcUrlService
{
    private ConcurrentBag<Domain.Entities.RpcUrl> _rpcUrls = new ConcurrentBag<Domain.Entities.RpcUrl>();

    public void AddRpcUrl(Chain chain, string rpcUrl)
    {
        _rpcUrls.Add(new Domain.Entities.RpcUrl { chain = chain, rpcUrl = rpcUrl });
    }

    public string? GetRpcUrl(Chain chain)
    {
        return _rpcUrls.FirstOrDefault(x => x.chain == chain)?.rpcUrl;
    }
}
