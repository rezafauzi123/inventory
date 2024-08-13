using System.Text;
using InventoryManagement.Shared.Abstractions.Cache;
using InventoryManagement.Shared.Abstractions.Serialization;
using Microsoft.Extensions.Localization;
using StackExchange.Redis;

namespace InventoryManagement.Shared.Infrastructure.Cache;

internal sealed class RedisCache : ICache
{
    private static readonly HashSet<Type> PrimitiveTypes = new()
    {
        typeof(string),
        typeof(char),
        typeof(int),
        typeof(long),
        typeof(Guid),
        typeof(decimal),
        typeof(double),
        typeof(float),
        typeof(short),
        typeof(uint),
        typeof(ulong)
    };

    private readonly IDatabase _database;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IStringLocalizer<RedisCache> _localizer;

    public RedisCache(IDatabase database, IJsonSerializer jsonSerializer, IStringLocalizer<RedisCache> localizer)
    {
        _database = database;
        _jsonSerializer = jsonSerializer;
        _localizer = localizer;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return default;
        }

        var value = await _database.StringGetAsync(key);
        return string.IsNullOrWhiteSpace(value) ? default : _jsonSerializer.Deserialize<T>(value!);
    }

    public async Task<IReadOnlyList<T>> GetManyAsync<T>(params string[] keys)
    {
        var values = new List<T>();
        if (!keys.Any())
        {
            return values;
        }

        var redisKeys = keys.Select(x => (RedisKey)x).ToArray();
        var redisValues = await _database.StringGetAsync(redisKeys);

        values.AddRange(from redisValue in redisValues
                        where redisValue.HasValue && !redisValue.IsNullOrEmpty
                        select _jsonSerializer.Deserialize<T>(redisValue!));

        return values;
    }

    public Task<string> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        decimal megabyteSize = ((decimal)Encoding.Unicode.GetByteCount(_jsonSerializer.Serialize(value)) / 1048576);
        var errorMessage = "";
        if (megabyteSize > 1m)
        {
            errorMessage = _localizer["to-large-text-size"];
            return Task.FromResult(errorMessage);
        }
        _database.StringSetAsync(key, _jsonSerializer.Serialize(value), expiry);
        return Task.FromResult(errorMessage);
    }
       

    public Task DeleteAsync(string key)
        => _database.KeyDeleteAsync(key);

    private string AsString<T>(T value)
        => (PrimitiveTypes.Contains(typeof(T)) ? value!.ToString() : _jsonSerializer.Serialize(value))!;
}