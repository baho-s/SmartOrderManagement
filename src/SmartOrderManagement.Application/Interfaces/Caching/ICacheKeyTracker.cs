using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Interfaces.Caching
{
    public interface ICacheKeyTracker
    {
        public void AddCacheKey(string cacheKey);
        public List<string> GetKeys();
        public void RemoveAll();//Bütün anahtarları sil.
        void RemoveKey(string cacheKey);//Belirli bir anahtarı sil.
        void RemoveByPrefix(string prefix);//Sadece belirli bir örnekle başlayan anahtarları sil.
    }
}
