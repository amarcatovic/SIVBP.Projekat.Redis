using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace Projekat.Front.Infrastructure.Caching
{
    public interface ICacheService
    {
        Task<bool> KeyExistsAsync(string key);
        Task Create<T>(T item, string key);
        Task<T> Get<T>(string key);
    }

    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;

        public CacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task Create<T>(T item, string key)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var db = _redis.GetDatabase();

            var serialItem = JsonSerializer.Serialize(item);
            await db.StringSetAsync(key, serialItem);
        }

        public async Task<T> Get<T>(string key)
        {
            var db = _redis.GetDatabase();
            var cachedItem = await db.StringGetAsync(key);

            if (string.IsNullOrEmpty(cachedItem))
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(cachedItem);
        }

        public async Task<bool> KeyExistsAsync(string key)
        {
            var db = _redis.GetDatabase();
            var result = await db.StringGetAsync(key);

            return !string.IsNullOrEmpty(result);
        }
    }
}
