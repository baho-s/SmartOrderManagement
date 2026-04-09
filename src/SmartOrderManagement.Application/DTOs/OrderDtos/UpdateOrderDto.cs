using SmartOrderManagement.Domain.Enums.OrderEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.DTOs.OrderDtos
{
    public class UpdateOrderDto
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        // Siparişin toplam tutarı
        public OrderStatus Status { get; set; }
        // Sipariş durumu (Hazırlanıyor, Tamamlandı, İptal)
        public bool PaymentStatus { get; set; }
        // Ödeme yapıldı mı yapılmadı mı

        public int CustomerId { get; set; }
        // Bu sipariş hangi müşteriye ait (Foreign Key)
    }
}
