using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.DTOs.ProductDtos
{
    public class ProductListDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public int ProductStock { get; set; }
        public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }
        // Bu ürün hangi kategoriye ait (Foreign Key)
        public string CategoryName { get; set; }

    }
}
