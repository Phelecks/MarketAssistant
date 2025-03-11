using BaseApplication.Interfaces;
using CacheManager;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace BaseApplication.Behaviour;

public class CachingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ICacheableMediatrQuery
{
	private readonly ILogger<TRequest> _logger;
	private readonly IDistributedCache _distributedCache;

    public CachingBehaviour(ILogger<TRequest> logger, IDistributedCache distributedCache)
	{
		_logger = logger;
		_distributedCache = distributedCache;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		TResponse response;
		if (request.bypassCache) return await next();

		async Task<TResponse> GetResponseAndAddToCache()
		{
			response = await next();
			var expireInMinutes = request.expireInMinutes ?? 10;
			await _distributedCache.SetAsync(key: request.cacheKey, 
					value: System.Text.Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(response)), 
					options: new() 
					{
						AbsoluteExpiration = DateTime.Now.AddMinutes(expireInMinutes)
					}, cancellationToken);
			return response;
		}

		var cacheResult = await _distributedCache.GetAsync(key: request.cacheKey, cancellationToken);
		if (cacheResult != null)
		{
			try
			{
				var result = System.Text.Json.JsonSerializer.Deserialize<TResponse>(cacheResult);
                if (result is null) return await next();
                response = result;
				_logger.LogInformation("Fetched from Cache -> '{CacheKey}'.", request.cacheKey);
            }
			catch (Exception)
			{
                return await next();
            }
		}
		else
		{
			response = await GetResponseAndAddToCache();
			_logger.LogInformation("Added to Cache -> '{CacheKey}'.", request.cacheKey);
		}
		return response;
	}
}