﻿using Nethereum.Signer;
using System.Collections.Concurrent;
using WalletTracker.Application.Interfaces;

namespace WalletTracker.Infrastructure.Services;

public class RpcUrlService : IRpcUrlService
{
    private ConcurrentBag<Domain.Entities.RpcUrl> _rpcUrls = new ConcurrentBag<Domain.Entities.RpcUrl>();

    public void AddRpcUrl(Domain.Entities.RpcUrl rpcUrl)
    {
        _rpcUrls.Add(rpcUrl);
    }

    public string GetRpcUrl(Chain chain)
    {
        return _rpcUrls.Single(x => x.chain == chain)?.rpcUrl;
    }
}
