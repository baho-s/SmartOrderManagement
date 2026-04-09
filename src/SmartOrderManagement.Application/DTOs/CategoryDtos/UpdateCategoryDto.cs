using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.DTOs.CategoryDtos
{
    public class UpdateCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public string CategoryDescription { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        // Üst kategori FK — root ise null
        public int? ParentCategoryId { get; set; } 
        //Navigation property'ler DTO'da olmaz, sadece FK'lar olur. ParentCategoryId ile ilişki kuracağız. ParentCategoryId null ise root kategori demektir.
        
    }
}
