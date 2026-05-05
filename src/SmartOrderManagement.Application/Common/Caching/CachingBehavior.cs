using MediatR;
using SmartOrderManagement.Application.Interfaces.Caching;

namespace SmartOrderManagement.Application.Common.Caching
{
    //TRequest: İstek türünü temsil eder, genellikle bir sorgu veya komut olabilir.(Yani bir Query veya Command göndereceğiz.)
    //TResponse: İstek sonucunda dönecek yanıt türünü temsil eder.(Biz mesela burada ProductListDto gibi bir DTO döneceğiz.)
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICacheService _cache;
        private readonly ICacheKeyTracker _cacheKeyTracker;

        public CachingBehavior(ICacheService cache, ICacheKeyTracker cacheKeyTracker)
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
            
            var cached=await _cache.GetAsync<TResponse>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                Console.WriteLine($"{DateTime.Now}Anahtara ait önbellek bulundu:.AAAAAAAAAAAAAAA {cacheKey}");
                return cached;
            }

            Console.WriteLine($"{DateTime.Now}.Anahtara ait önbellek bulunamadı:{cacheKey}. Handler'a yönlendiriliyor.");

            var response = await next();
            if(response is not null)
            {
                //1) Asıl response'u kendi key'i ile cache'e yaz.
                //Örn: products-1-10 → List<ProductListDto>
                await _cache.SetAsync(cacheKey, response, cacheableQuery.AbsoluteExpiration, cancellationToken);
                await _cacheKeyTracker.AddCacheKeyAsync(cacheKey);

                //2) Eğer bu request aynı zamanda ICacheSeedingQuery ise,
                //OnCached metodunu çağır.
                //Behavior burada ProductListDto'yu TANIMAZ.
                //Sadece "sen seed yapabiliyorsan yap" der.
                //Asıl seed mantığı Query sınıfının içindedir.
                if (request is ICacheSeedingQuery<TResponse> seedingQuery)
                {
                    seedingQuery.OnCached(response, _cache, _cacheKeyTracker);
                }
                // Toplam cache sayısını her yazma işleminden sonra bas
                var total = (await _cacheKeyTracker.GetKeysAsync()).Count;
                Console.WriteLine($"[Cache] Toplam kayıt: {total} | Son eklenen: {cacheKey}");
            }
            return response;

        }
    }
}
