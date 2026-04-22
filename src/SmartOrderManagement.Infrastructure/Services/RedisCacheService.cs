using Microsoft.Extensions.Caching.Distributed;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartOrderManagement.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
        };

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var data = await _cache.GetStringAsync(key, cancellationToken);
            if (string.IsNullOrEmpty(data))
                return default;

            return JsonSerializer.Deserialize<T>(data, _jsonOptions);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10)
            };

            var json = JsonSerializer.Serialize(value, _jsonOptions);
            await _cache.SetStringAsync(key, json, options, cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(key, cancellationToken);
        }

        public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
        {
            var data = await _cache.GetAsync(key, cancellationToken);
            return data != null;
        }
    }
}
