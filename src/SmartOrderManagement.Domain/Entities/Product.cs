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
    }
}
