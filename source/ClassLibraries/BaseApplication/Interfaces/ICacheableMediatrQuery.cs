namespace BaseApplication.Interfaces;

public interface ICacheableMediatrQuery
{
	bool bypassCache { get; }
	string cacheKey { get; }
	int? expireInMinutes { get; }
}