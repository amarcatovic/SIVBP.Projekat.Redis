using Projekat.Front.Dtos;
using Projekat.Front.Infrastructure.Persistence.Models;
using Projekat.Front.Utilities;
using StackExchange.Redis;
using System.Text.Json;

namespace Projekat.Front.Infrastructure.Caching
{
    public interface ICacheService
    {
        Task<bool> KeyExistsAsync(string key);
        Task Create<T>(T item, string key);
        Task<T> Get<T>(string key);
        Task UpdateLatestPostCacheAsync(PostReadDto postReadDto);
        Task UpdateCachedPostViewCountAsync(int postId, int viewCount);
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

        public async Task UpdateCachedPostViewCountAsync(int postId, int viewCount)
        {
            var db = _redis.GetDatabase();
            var cachedItems = await db.StringGetAsync(Constants.LATEST_POSTS_KEY);

            if (string.IsNullOrEmpty(cachedItems))
            {
                return;
            }

            var cache = JsonSerializer.Deserialize<List<PostReadDto>>(cachedItems);
            var item = cache.FirstOrDefault(p => p.Id == postId);

            if (item == null)
            {
                return;
            }

            item.ViewCount = viewCount;

            var serialItem = JsonSerializer.Serialize(cache);
            await db.StringSetAsync(Constants.LATEST_POSTS_KEY, serialItem);
        }

        public async Task UpdateLatestPostCacheAsync(PostReadDto postReadDto)
        {
            var db = _redis.GetDatabase();
            var cachedItems = await db.StringGetAsync(Constants.LATEST_POSTS_KEY);

            if (string.IsNullOrEmpty(cachedItems))
            {
                return;
            }

            var cache = JsonSerializer.Deserialize<List<PostReadDto>>(cachedItems);

            cache.Insert(0, postReadDto);
            cache = cache
                .Take(10)
                .ToList();

            var serialItem = JsonSerializer.Serialize(cache);
            await db.StringSetAsync(Constants.LATEST_POSTS_KEY, serialItem);
        }
    }
}
