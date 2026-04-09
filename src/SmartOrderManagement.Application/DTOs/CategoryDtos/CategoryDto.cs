using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.DTOs.CategoryDtos
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string? ImageUrl { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
