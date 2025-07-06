using Microsoft.Extensions.Caching.Memory;

namespace back.Utilities;

public class CacheManager : ICacheManager
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<CacheManager> _logger;

    public CacheManager(IMemoryCache memoryCache, ILogger<CacheManager> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public T? Get<T>(string key) where T : class
    {
        if (_memoryCache.TryGetValue(key, out T? value))
        {
            _logger.LogDebug("Cache hit for key: {Key}", key);
            return value;
        }
        
        _logger.LogDebug("Cache miss for key: {Key}", key);
        return null;
    }

    public void Set<T>(string key, T value, TimeSpan expiration) where T : class
    {
        _memoryCache.Set(key, value, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        });
        
        _logger.LogDebug("Cached item with key: {Key} for {ExpirationMinutes} minutes", 
            key, expiration.TotalMinutes);
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
        _logger.LogDebug("Removed cached item with key: {Key}", key);
    }
}

public interface ICacheManager
{
    T? Get<T>(string key) where T : class;
    void Set<T>(string key, T value, TimeSpan expiration) where T : class;
    void Remove(string key);
}
