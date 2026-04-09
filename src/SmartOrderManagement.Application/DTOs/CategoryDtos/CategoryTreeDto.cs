using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.DTOs.CategoryDtos
{
    public class CategoryTreeDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ParentCategoryId { get; set; }

        // Sadece üst kategorinin adını göster
        public string? ParentCategoryName { get; set; }

        // Alt kategoriler recursive olarak gelsin
        public List<CategoryTreeDto> SubCategories { get; set; } = new();

        // Ürün sayısını göster (tüm ürünleri göndermek yerine)
        public int ProductCount { get; set; }
    }
}
