using System.Text;
using InventoryManagement.Shared.Abstractions.Cache;
using InventoryManagement.Shared.Abstractions.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace InventoryManagement.Shared.Infrastructure.Cache;

public class InMemoryCache : ICache
{
    private readonly IMemoryCache _memoryCache;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IStringLocalizer<InMemoryCache> _localizer;

    public InMemoryCache(IMemoryCache memoryCache, IJsonSerializer jsonSerializer, IStringLocalizer<InMemoryCache> localizer)
    {
        _memoryCache = memoryCache;
        _jsonSerializer = jsonSerializer;
        _localizer = localizer;
    }

    public Task<T?> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return Task.FromResult<T?>(default);

        var value = _memoryCache.Get<string>(key);

        return value is null
            ? Task.FromResult<T?>(default)
            : Task.FromResult(_jsonSerializer.Deserialize<T>(value));
    }

    public Task<IReadOnlyList<T>> GetManyAsync<T>(params string[] keys)
    {
        var values = new List<T>();

        if (!keys.Any())
            return Task.FromResult<IReadOnlyList<T>>(values);

        foreach (var item in keys)
        {
            var value = _memoryCache.Get<string>(item);
            if (!string.IsNullOrWhiteSpace(value))
                values.Add(_jsonSerializer.Deserialize<T>(value)!);
        }

        return Task.FromResult<IReadOnlyList<T>>(values);
    }

    public Task<string> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        decimal megabyteSize = ((decimal)Encoding.Unicode.GetByteCount(_jsonSerializer.Serialize(value)) / 1048576);
        var errorMessage = "";
        if (megabyteSize > 1m )
        {
            errorMessage = _localizer["to-large-text-size"];
            return Task.FromResult(errorMessage);
        }
        _memoryCache.Set(key, _jsonSerializer.Serialize(value), expiry ?? TimeSpan.FromMinutes(3));
        return Task.FromResult(errorMessage);
    }

    public Task DeleteAsync(string key)
    {
        _memoryCache.Remove(key);
        return Task.CompletedTask;
    }
}