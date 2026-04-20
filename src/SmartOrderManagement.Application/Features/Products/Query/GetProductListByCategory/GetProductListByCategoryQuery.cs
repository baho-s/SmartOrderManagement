using MediatR;
using Microsoft.Extensions.Caching.Memory;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Features.Products.Query.GetProductListByCategory
{
    public class GetProductListByCategoryQuery : IRequest<List<ProductListDto>>, ICacheSeedingQuery<List<ProductListDto>>
    {
        public int CategoryId { get; init; }
        public string CacheKey => $"products-category-{CategoryId}";
        public TimeSpan AbsoluteExpiration => TimeSpan.FromSeconds(240);

        //Bu method sayesinde toplu bir listeleme yaptığımızda daha sonradan sadece Id'ile bir ürün aranırsa
        //Aynı zamanda id'lerine göre ayırıp'ta listeleme yapıyoruz.
        //Bu cache'i biraz gereksiz şişirebilir ancak bu senaryo aklıma gelince nasıl yaparız diye düşünüp 
        //eklemek istedim. Mantığıda anlayabilmek için.
        public void OnCached(List<ProductListDto> response, IMemoryCache cache, ICacheKeyTracker tracker)
        {
            foreach (var product in response)
            {                
                cache.Set($"product-{product.ProductId}", product, AbsoluteExpiration);
                tracker.AddCacheKey($"product-{product.ProductId}");
            }
        }
    }
}
