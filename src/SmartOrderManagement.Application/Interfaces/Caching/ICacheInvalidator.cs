using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Interfaces.Caching
{
    public interface ICacheInvalidator
    {
        //Hangi cache key'ler silinecek
        List<string> CacheKeysToRemove { get; }
    }
}
