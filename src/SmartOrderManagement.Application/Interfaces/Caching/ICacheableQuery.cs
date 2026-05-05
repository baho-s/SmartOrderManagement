using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Interfaces.Caching
{
    public interface ICacheableQuery
    {
        // Cache anahtarı, değişken ismi gibi binevi Cache'in benzersiz tanımlayıcısıdır.
        string CacheKey { get; }
        // Cache süresi, verinin ne kadar süreyle cache'de tutulacağını belirler.
        TimeSpan AbsoluteExpiration { get; }
    }
}
