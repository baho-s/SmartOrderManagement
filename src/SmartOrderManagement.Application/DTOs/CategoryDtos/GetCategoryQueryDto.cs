using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.DTOs.CategoryDtos
{
    public class GetCategoryQueryDto
    {
        public int Page { get; set; } = 1;
        // Kaçıncı sayfanın istendiğini tutar
        // Default 1 verdik çünkü kullanıcı parametre göndermezse ilk sayfa gelsin

        public int PageSize { get; set; } = 10;
        // Her sayfada kaç kayıt olacağını belirler
        // Default 10 verdik (gerçek projelerde 10-20 yaygındır)

        public string? Search { get; set; }
        // Arama için kullanılacak (şimdilik kullanmayacağız ama altyapıyı hazırlıyoruz)

        public string? SortBy { get; set; }
        // Hangi alana göre sıralama yapılacak (örn: name, id)

        public string? SortDirection { get; set; }
        // Sıralama yönü (asc / desc)
    }
}
