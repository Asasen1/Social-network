using System.Collections.Concurrent;
using System.Text.Json;
using Application.Providers;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Providers;

public class CacheProvider : ICacheProvider
{
    private readonly IDistributedCache _cache;
    private static ConcurrentDictionary<string, bool> caheKyes = new();

    public CacheProvider(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        where T : class
    {
        var cachedValue = await _cache.GetStringAsync(key, token: ct);

        if (cachedValue is null)
            return null;

        var value = JsonSerializer.Deserialize<T>(cachedValue);
        return value;
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken ct = default)
    {
        var stringValue = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, stringValue, ct);
        caheKyes.TryAdd(key, true);
    }

    public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> factory, CancellationToken ct = default)
        where T : class
    {
        var cachedValue = await GetAsync<T>(key, ct);
        if (cachedValue is not null)
            return cachedValue;

        cachedValue = await factory();
        await SetAsync(key, cachedValue, ct);

        return cachedValue;
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default)
    {
        await _cache.RemoveAsync(key, ct);
        caheKyes.TryRemove(key, out _);
    }
    
    public async Task RemoveByPrefixAsync<T>(string key, string prefix, CancellationToken ct = default)
    {
        var tasks = caheKyes.Keys
            .Where(k => k.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase))
            .Select(k => RemoveAsync(k, ct));

        await Task.WhenAll(tasks);
    }
}