using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Interfaces.Caching
{
    // Seeding destekleyen sorgular için ayrı arayüz
    public interface ICacheSeedingQuery<TResponse> : ICacheableQuery
    {
        Task OnCached(TResponse response, ICacheService cache, ICacheKeyTracker tracker);

    }
}
