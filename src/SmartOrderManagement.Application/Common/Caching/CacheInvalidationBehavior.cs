using MediatR;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Memory;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Common.Caching
{
    public class CacheInvalidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICacheKeyTracker _cacheKeyTracker;

        public CacheInvalidationBehavior(ICacheKeyTracker cacheKeyTracker)
        {
            _cacheKeyTracker = cacheKeyTracker;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            //Eğer istek ICacheInvalidator arayüzünü uyguluyorsa, belirtilen cache anahtarlarını temizle
            if (request is ICacheInvalidator invalidator)
            {
                foreach (var cacheKey in invalidator.CacheKeysToRemove)
                {
                    _cacheKeyTracker.RemoveByPrefix(cacheKey);
                    Console.WriteLine($"CACHE Temizlendi → {cacheKey}");
                }
            }
            return response;
        }
    }
}
