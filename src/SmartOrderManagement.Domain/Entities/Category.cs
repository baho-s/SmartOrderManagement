using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SmartOrderManagement.Domain.Entities
{
    public class Category : BaseEntity
    {
        // PK
        public int CategoryId { get; set; }

        // Temel özellikler
        public string CategoryName { get; private set; } = string.Empty;

        public string CategoryDescription { get; private set; } = string.Empty;

        public string? ImageUrl { get; private set; }
        public bool IsActive { get; private set; } = true;


        // Üst kategori FK — root ise null
        public int? ParentCategoryId { get; private set; }

        //Her kategorinin bir alt kategorisi olabilir veri yönetimi kolaylaştırırız.
        //ParentCategory bir kategorinin üst kategorisini gösterir.
        // Üst kategori navigation
        public virtual Category? ParentCategory { get; private set; }//Bu bir navigation property'dir ve bir kategorinin üst kategorisini temsil eder. Eğer bir kategori root ise, ParentCategory null olacaktır.


        // Alt kategoriler navigation
        public virtual ICollection<Category> SubCategories { get; private set; } = new HashSet<Category>();

        // Bir kategori birçok ürüne sahip olabilir
        public virtual ICollection<Product> Products { get; private set; } = new HashSet<Product>();


        public Category()
        {

        }

        public Category(string categoryName, string categoryDescription, string? imageUrl, bool isActive, int? parentCategoryId)
        {
            if (CategoryId <= 0)
            {
                throw new Exception("Kategori ID'si sıfırdan büyük olmalıdır.");
            }

            if (parentCategoryId.HasValue && parentCategoryId <= 0)
            {
                throw new Exception("Eğer ParentCategoryId sağlanıyorsa, sıfırdan büyük olmalıdır.");
            }

            CategoryName = categoryName;
            CategoryDescription = categoryDescription;
            ImageUrl = imageUrl;
            IsActive = isActive;
            ParentCategoryId = parentCategoryId;
        }


        public void UpdateCategoryNameAndDescription(string newCategoryName, string newCategoryDescription)
        {
            CategoryName = newCategoryName;
            CategoryDescription = newCategoryDescription;
        }

        public void UpdateCategoryParentId(int? newParentCategoryId)
        {
            if (newParentCategoryId.HasValue && newParentCategoryId <= 0)
            {
                throw new Exception("ParentCategoryId pozitif olmalıdır.");
            }
            ParentCategoryId = newParentCategoryId;
        }

        public void DeleteCategory()
        {
            IsDeleted = true;
            IsActive = false;
        }

        public void MoveChildCategory(int newParentCategoryId)
        {
            ParentCategoryId = newParentCategoryId;
        }
    }

}
