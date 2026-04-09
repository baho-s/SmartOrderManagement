using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Domain.Entities
{
    public class Product:BaseEntity
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }= string.Empty;
        public decimal ProductPrice { get; set; }
        public int ProductStock { get; set; }
        public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }
        // Bu ürün hangi kategoriye ait (Foreign Key)
        public virtual Category Category { get; set; } = null!;
        // Ürünün bağlı olduğu kategori (Navigation Property)

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        // Bu ürün birçok siparişte yer alabilir

    }
}
