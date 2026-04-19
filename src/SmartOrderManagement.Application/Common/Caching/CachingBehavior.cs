using MediatR;
using Microsoft.Extensions.Caching.Memory;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Common.Caching
{
    //TRequest: İstek türünü temsil eder, genellikle bir sorgu veya komut olabilir.(Yani bir Query veya Command göndereceğiz.)
    //TResponse: İstek sonucunda dönecek yanıt türünü temsil eder.(Biz mesela burada ProductListDto gibi bir DTO döneceğiz.)
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IMemoryCache _cache;
        private readonly ICacheKeyTracker _cacheKeyTracker;

        public CachingBehavior(IMemoryCache cache, ICacheKeyTracker cacheKeyTracker)
        {
            _cache = cache;
            _cacheKeyTracker = cacheKeyTracker;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            //Bu request'in ICacheableQuery arayüzünü implemente edip etmediğini kontrol ediyoruz.
            if (request is not ICacheableQuery cacheableQuery)
            {
                return await next();
            }

            var cacheKey = cacheableQuery.CacheKey;

            if (_cache.TryGetValue(cacheKey, out TResponse cachedResponse))
            {
                Console.WriteLine($"{DateTime.Now}Anahtara ait önbellek bulundu:.AAAAAAAAAAAAAAA {cacheKey}");
                return cachedResponse;
            }

            Console.WriteLine($"{DateTime.Now}.Anahtara ait önbellek bulunamadı:{cacheKey}. Handler'a yönlendiriliyor.");

            var response = await next();
            if(response is not null)
            {
                //1) Asıl response'u kendi key'i ile cache'e yaz.
                //Örn: products-1-10 → List<ProductListDto>
                _cache.Set(cacheKey, response,cacheableQuery.AbsoluteExpiration);
                _cacheKeyTracker.AddCacheKey(cacheKey);

                //2) Eğer bu request aynı zamanda ICacheSeedingQuery ise,
                //OnCached metodunu çağır.
                //Behavior burada ProductListDto'yu TANIMAZ.
                //Sadece "sen seed yapabiliyorsan yap" der.
                //Asıl seed mantığı Query sınıfının içindedir.
                if (request is ICacheSeedingQuery<TResponse> seedingQuery)
                {
                    seedingQuery.OnCached(response, _cache, _cacheKeyTracker);
                }
            }
            var allKeys= _cacheKeyTracker.GetKeys();
            foreach (var key in allKeys)
            {
                Console.WriteLine($"Cache Key: {key}");
            }
            return response;

        }
    }
}
