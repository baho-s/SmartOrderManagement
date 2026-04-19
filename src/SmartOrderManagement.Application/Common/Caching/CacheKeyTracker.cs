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
        private readonly IMemoryCache _cache;
        private const string CacheKeyRegistry = "DENEME_KEY_APP";

        public CacheKeyTracker(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void AddCacheKey(string cacheKey)
        {
            var cacheKeys = GetKeys(); 
            if (!cacheKeys.Contains(cacheKey))
            {
                cacheKeys.Add(cacheKey);
                _cache.Set(CacheKeyRegistry, cacheKeys);
            }
            Console.WriteLine($"Cache'te OnCached'den yeni oluşturulan key:{cacheKey}");
        }

        //Tüm key’leri getir
        public List<string> GetKeys()
        {
            return _cache.Get<List<string>>(CacheKeyRegistry) ?? new List<string>();
        }

        //Hepsini temizle
        public void RemoveAll()
        {
            var keys = GetKeys();

            foreach (var key in keys)  
            {
                _cache.Remove(key);
            }

            // listeyi de temizle
            _cache.Remove(CacheKeyRegistry);
        }

        //Bu key'i temizle
        public void RemoveKey(string cacheKey)
        {
            _cache.Remove(cacheKey); // Fiziksel olarak cache'ten sil

            // Defterden de bu ismi silelim
            var keys = GetKeys();
            if (keys.Remove(cacheKey))
            {                
                _cache.Set(CacheKeyRegistry, keys);
            }
        }

        public void RemoveByPrefix(string prefix)
        {
            var allKeys= GetKeys();

            // 1. Silinecekleri belirle
            var keysToRemove = allKeys.Where(k => k.StartsWith(prefix)).ToList();

            if (!keysToRemove.Any()) { return; } //Silinecek anahtar yoksa işlemi sonlandır

            // 2. Fiziksel cache'leri temizle
            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
                allKeys.Remove(key); // Defterdeki listeden de uçur
            }

            // 3. EN ÖNEMLİSİ: Defteri tek seferde güncelle (Döngü bittikten sonra!)
            _cache.Set(CacheKeyRegistry, allKeys);  
        }
    }
}
