namespace BaseApplication.Interfaces;

public interface ICacheableAddDomainNotificationQuery
{
	bool bypassCache { get; }
	string cacheKey { get; }
	int? expireInMinutes { get; }
}