using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.DTOs.CategoryDtos
{
    public class CreateCategoryDto
    {

        // Temel özellikler
        public string CategoryName { get; set; } = string.Empty;

        public string CategoryDescription { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        // Üst kategori FK — root ise null
        public int? ParentCategoryId { get; set; } //EĞer id'si 0 olarak gelirse service katmanında null yapıyoruz null durumda kök kategorimiz oluyor.

    }
}
