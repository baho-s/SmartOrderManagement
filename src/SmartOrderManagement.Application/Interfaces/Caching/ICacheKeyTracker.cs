using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Interfaces.Caching
{
    public interface ICacheKeyTracker
    {
        public Task AddCacheKeyAsync(string cacheKey);
        public Task<List<string>> GetKeysAsync();
        public Task RemoveAllAsync();//Bütün anahtarları sil.
        Task RemoveKeyAsync(string cacheKey);//Belirli bir anahtarı sil.
        Task RemoveByPrefixAsync(string prefix);//Sadece belirli bir örnekle başlayan anahtarları sil.
    }
}
