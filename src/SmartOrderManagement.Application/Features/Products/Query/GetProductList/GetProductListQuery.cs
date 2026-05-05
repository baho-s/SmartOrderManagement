using MediatR;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using SmartOrderManagement.Application.Interfaces.Caching;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SmartOrderManagement.Application.Features.Products.Query.GetProductList
{
    public class GetProductListQuery:IRequest<List<ProductListDto>>, ICacheSeedingQuery<List<ProductListDto>>
    {
        public byte PageNumber { get; set; } = 1;
        public byte PageSize { get; set; } = 10;

        [BindNever]        
        public string CacheKey => $"products-{PageNumber}-{PageSize}";

        [BindNever]        
        public TimeSpan AbsoluteExpiration => TimeSpan.FromSeconds(240);

        //YENİ METOD
        // CachingBehavior bu metodu çağırdığında "products-1-10" cache'e yazılmış demek.
        // Şimdi listedeki her ürünü ayrı ayrı product-{id} key'iyle de cache'e yazıyoruz.
        //
        // Neden bu mantık Behavior'da değil burada?
        // Çünkü bu bir "Product davranışı". Yarın OrderListQuery gelirse
        // o kendi OnCached'ını yazar, Behavior'a dokunmadan.
        public async Task OnCached(List<ProductListDto> response, ICacheService cache, ICacheKeyTracker tracker)
        {
            foreach (var item in response)
            {
                // GetProductByIdQuery'nin CacheKey formatıyla birebir aynı olmalı!
                // Tutarsızlık olursa seed ettiğimiz key hiç bulunamaz.
                var cacheKey = $"product-{item.ProductId}";

                // ProductByIdDto oluşturuyoruz çünkü GetProductByIdQuery bu tipi bekliyor.
                // Behavior ileride product-40 key'ine baktığında bunu bulacak ve
                // doğrudan dönebilecek.
                var dto = new 
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    ProductPrice = item.ProductPrice,
                    ProductStock = item.ProductStock,
                    CategoryId = item.CategoryId,
                    IsActive = item.IsActive,
                    CategoryName = item.CategoryName
                };

                // Liste ile aynı süre geçerli olsun (240 sn).
                await cache.SetAsync(cacheKey, dto, AbsoluteExpiration);
                await tracker.AddCacheKeyAsync(cacheKey);                
            }
        }
    }
}
