using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.BLL.Interfaces;

namespace Talabat.BLL.Services
{
    public class ResponceCacheService : IResponceCacheService
    {
        private readonly IDatabase _database;

        public ResponceCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task CacheResponceAsync(string cacheKey, object responce, TimeSpan timeToLive)
        {
            if (responce is null) return;
            
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var serializedResponce = JsonSerializer.Serialize(responce, options);

            await _database.StringSetAsync(cacheKey , serializedResponce, timeToLive);
        }

        public async Task<string> GetCachedResponce(string cacheKey)
        {
            var cachedResponce = await _database.StringGetAsync(cacheKey);

            if (cachedResponce.IsNullOrEmpty) return null;

            return cachedResponce;
        }
    }
}
