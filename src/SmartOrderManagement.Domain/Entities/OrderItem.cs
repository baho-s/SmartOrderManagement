using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Domain.Entities
{
    public class OrderItem:BaseEntity
    {
        public int OrderItemId { get; set; }

        public int Quantity { get; set; }
        // Bu üründen kaç adet sipariş verildi
        public decimal UnitPrice { get; set; }
        // Ürünün sipariş anındaki birim fiyatı
        public decimal TotalPrice { get; set; }
        // Bu satırın toplam fiyatı (Quantity * UnitPrice)

        public int OrderId { get; set; }
        // Bu sipariş satırı hangi siparişe ait (Foreign Key)
        public Order Order { get; set; } = null!;
        // Bu satırın bağlı olduğu sipariş (Navigation Property)

        public int ProductId { get; set; }
        // Bu sipariş satırı hangi ürüne ait (Foreign Key)
        public virtual Product Product { get; set; }=null!;
        // Bu sipariş satırının bağlı olduğu ürün

    }
}
