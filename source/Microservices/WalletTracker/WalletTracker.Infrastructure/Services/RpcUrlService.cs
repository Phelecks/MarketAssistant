using Nethereum.Signer;
using System.Collections.Concurrent;
using WalletTracker.Application.Interfaces;

namespace WalletTracker.Infrastructure.Services;

public class RpcUrlService : IRpcUrlService
{
    private readonly ConcurrentBag<Domain.Entities.RpcUrl> _rpcUrls = [];

    public void AddRpcUrl(Domain.Entities.RpcUrl rpcUrl)
    {
        _rpcUrls.Add(rpcUrl);
    }

    public string? GetRpcUrl(Chain chain)
    {
        return _rpcUrls.Single(x => x.Chain == chain)?.Uri.ToString();
    }
}
