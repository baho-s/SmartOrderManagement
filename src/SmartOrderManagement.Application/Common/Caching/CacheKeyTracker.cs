using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Win32;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Common.Caching
{
    public class CacheKeyTracker:ICacheKeyTracker
    {
        private readonly ICacheService _cache;
        private const string CacheKeyRegistry = "DENEME_KEY_APP";

        public CacheKeyTracker(ICacheService cache)
        {
            _cache = cache;
        }

        public async Task AddCacheKeyAsync(string cacheKey)
        {
            var cacheKeys = await GetKeysAsync(); 
            if (!cacheKeys.Contains(cacheKey))
            {
                cacheKeys.Add(cacheKey);
                await _cache.SetAsync(CacheKeyRegistry, cacheKeys);
                Console.WriteLine($"Cache'te OnCached'den yeni oluşturulan key:{cacheKey}");
            }
            
        }

        //Tüm key’leri getir
        public async Task<List<string>> GetKeysAsync()
        {
            return await _cache.GetAsync<List<string>>(CacheKeyRegistry) ?? new List<string>();
        }

        //Hepsini temizle
        public async Task RemoveAllAsync()
        {
            var keys = await GetKeysAsync();

            foreach (var key in keys)  
            {
                await _cache.RemoveAsync(key);
            }

            // listeyi de temizle
            await _cache.RemoveAsync(CacheKeyRegistry);
        }

        //Bu key'i temizle
        public async Task RemoveKeyAsync(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey); // Fiziksel olarak cache'ten sil

            // Defterden de bu ismi silelim
            var keys = await GetKeysAsync();
            if (keys.Remove(cacheKey))
            {                
                await _cache.SetAsync(CacheKeyRegistry, keys);
            }
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            var allKeys= await GetKeysAsync();

            // 1. Silinecekleri belirle
            var keysToRemove = allKeys.Where(k => k.StartsWith(prefix)).ToList();

            if (!keysToRemove.Any()) { return; } //Silinecek anahtar yoksa işlemi sonlandır

            // 2. Fiziksel cache'leri temizle
            foreach (var key in keysToRemove)
            {
                await _cache.RemoveAsync(key);
                allKeys.Remove(key); // Defterdeki listeden de uçur
            }

            // 3. EN ÖNEMLİSİ: Defteri tek seferde güncelle (Döngü bittikten sonra!)
            await _cache.SetAsync(CacheKeyRegistry, allKeys);  
        }
    }
}
