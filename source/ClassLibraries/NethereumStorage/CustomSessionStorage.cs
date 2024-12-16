using Microsoft.Extensions.Caching.Distributed;
using Nethereum.Siwe;
using Nethereum.Siwe.Core;


namespace NethereumStorage;

public class CustomSessionStorage : ISessionStorage
{
    private readonly IDistributedCache _distributedCache;
    const int DefaultCacheExpireTimeInMinutes = 60;

    public CustomSessionStorage(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public void AddOrUpdate(SiweMessage siweMessage)
    {
        var siweIssueTime = siweMessage.GetIssuedAtAsDateTime();
        var siweExpirationTime = siweMessage.GetExpirationTimeAsDateTime();
        var expireTimeInMinutes = (int)(siweExpirationTime - siweIssueTime).TotalMinutes;
        if(expireTimeInMinutes <= 0)
            expireTimeInMinutes = DefaultCacheExpireTimeInMinutes;
        
        _distributedCache.Set(key: $"{CacheManager.Helpers.CacheKeys.NethereumSessionStoragePrefix}{siweMessage.Nonce}", 
            value: System.Text.Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(siweMessage)), 
            options: new() 
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(expireTimeInMinutes)
            });
    }

    public SiweMessage GetSiweMessage(SiweMessage siweMessage)
    {
        var cacheResult = _distributedCache.Get(key: $"{CacheManager.Helpers.CacheKeys.NethereumSessionStoragePrefix}{siweMessage.Nonce}");
        if(cacheResult is null) return new SiweMessage();
        var result = System.Text.Json.JsonSerializer.Deserialize<SiweMessage>(cacheResult);
        if (result is null) return new SiweMessage();
        return result;
    }

    public void Remove(SiweMessage siweMessage)
    {
        _distributedCache.Remove($"{CacheManager.Helpers.CacheKeys.NethereumSessionStoragePrefix}{siweMessage.Nonce}");
    }

    public void Remove(string nonce)
    {
        _distributedCache.Remove($"{CacheManager.Helpers.CacheKeys.NethereumSessionStoragePrefix}{nonce}");
    }
}