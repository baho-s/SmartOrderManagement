using SmartOrderManagement.Domain.Enums.OrderEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Domain.Entities
{
    public class Order:BaseEntity
    {
        
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }=DateTime.Now;
        // Siparişin oluşturulduğu tarih
        public decimal TotalAmount { get; set; }
        // Siparişin toplam tutarı
        public OrderStatus Status { get; set; } 
        // Sipariş durumu (Hazırlanıyor, Tamamlandı, İptal)
        public bool PaymentStatus { get; set; }
        // Ödeme yapıldı mı yapılmadı mı

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        // Bir siparişte birden fazla ürün olabilir

        public int CustomerId { get; set; }
        // Bu sipariş hangi müşteriye ait (Foreign Key)
        public virtual Customer Customer { get; set; }
        // Siparişi veren müşteri (Navigation Property)


    }
}
