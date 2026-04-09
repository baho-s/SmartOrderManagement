using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Domain.Entities
{
    public class Category:BaseEntity
    {
        // PK
        public int CategoryId { get; set; }

        // Temel özellikler
        public string CategoryName { get; set; }=string.Empty;

        public string CategoryDescription { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;


        // Üst kategori FK — root ise null
        public int? ParentCategoryId { get; set; }
        
        //Her kategorinin bir alt kategorisi olabilir veri yönetimi kolaylaştırırız.
        //ParentCategory bir kategorinin üst kategorisini gösterir.
        // Üst kategori navigation
        public virtual Category? ParentCategory { get; set; }//Bu bir navigation property'dir ve bir kategorinin üst kategorisini temsil eder. Eğer bir kategori root ise, ParentCategory null olacaktır.


        // Alt kategoriler navigation
        public virtual ICollection<Category> SubCategories { get; set; } = new HashSet<Category>();


        // Bir kategori birçok ürüne sahip olabilir
        public virtual ICollection<Product> Products { get; set; }=new HashSet<Product>();
    }
}
