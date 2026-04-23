using MediatR;
using SmartOrderManagement.Application.Interfaces.Caching;
using System.Text.Json.Serialization;

namespace SmartOrderManagement.Application.Features.Products.Command.UpdateProductStockAndPrice
{
    public class UpdateProductStockAndPriceCommand:IRequest,ICacheInvalidator
    {
        [JsonIgnore]
        public int ProductId { get; set; }
        public decimal NewPrice { get; init; }
        public int NewStock { get; init; }

        public List<string> CacheKeysToRemove => new()
        {
                $"product-{ProductId}"
        };
    }
}
