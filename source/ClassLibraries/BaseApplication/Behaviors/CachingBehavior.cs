using BaseApplication.Interfaces;
using MediatR.Attributes;
using MediatR.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace BaseApplication.Behavior;

[BehaviorOrder(5)]
public class CachingBehavior<TRequest, TResponse>(ILogger<CachingBehavior<TRequest, TResponse>> logger, IDistributedCache distributedCache) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ICacheableAddDomainNotificationQuery
{
	private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger = logger;
	private readonly IDistributedCache _distributedCache = distributedCache;

	public async Task<TResponse> HandleAsync(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken)
	{
		TResponse response;
		if (request.bypassCache) return await next(cancellationToken);

		async Task<TResponse> GetResponseAndAddToCache()
		{
			response = await next(cancellationToken);
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
				if (result is null) return await next(cancellationToken);
				response = result;
				_logger.LogInformation("Fetched from Cache -> '{CacheKey}'.", request.cacheKey);
			}
			catch (Exception)
			{
				return await next(cancellationToken);
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