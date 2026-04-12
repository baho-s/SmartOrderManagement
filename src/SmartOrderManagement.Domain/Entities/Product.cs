using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Domain.Entities
{
    public class Product:BaseEntity
    {
        public int ProductId { get; private set; }

        public string ProductName { get; private set; }= string.Empty;
        public decimal ProductPrice { get; private set; }
        public int ProductStock { get; private set; }
        public bool IsActive { get; private set; } = true;
        public int CategoryId { get; private set; }
        // Bu ürün hangi kategoriye ait (Foreign Key)
        public virtual Category Category { get; set; } = null!;
        // Ürünün bağlı olduğu kategori (Navigation Property)

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        // Bu ürün birçok siparişte yer alabilir

        public void DecreaseStock(int quantity)
        {
            if(quantity <= 0)
            {
                throw new ArgumentException("Miktar sıfırdan büyük olmalıdır.");
            }
            if(quantity > ProductStock)
            {
                throw new InvalidOperationException("Yeterli stok yok.");
            }
            ProductStock -= quantity;
        }

        public Product(string productName, decimal productPrice, int productStock, bool isActive, int categoryId)
        {
            ProductName = productName;
            ProductPrice = productPrice;
            ProductStock = productStock;
            IsActive = isActive;
            CategoryId = categoryId;
        }

        public void UpdateProductIsActive(bool newIsActive)
        {
            IsActive = newIsActive;
        }

        public void UpdateProductCategoryId(int newCategoryId)
        {
            if(newCategoryId <= 0)
            {
                throw new ArgumentException("Kategori ID sıfırdan büyük olmalıdır.");
            }
            CategoryId = newCategoryId;
        }

        public void UpdateProductName(string newProductName)
        {
            if(string.IsNullOrWhiteSpace(newProductName))
            {
                throw new ArgumentException("Ürün adı boş olamaz.");
            }
            ProductName = newProductName;
        }

        public void UpdateProductStockAndPrice(decimal newPrice, int newStock)
        {
            if (newPrice < 0)
            {
                throw new ArgumentException("Fiyat negatif olamaz.");
            }
            if (newStock < 0)
            {
                throw new ArgumentException("Stok negatif olamaz.");
            }
            ProductPrice = newPrice;
            ProductStock = newStock;
        }
    }
}
