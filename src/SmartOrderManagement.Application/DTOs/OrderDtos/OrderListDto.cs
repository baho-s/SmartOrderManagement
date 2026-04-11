using SmartOrderManagement.Application.DTOs.OrderItemDtos;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Domain.Enums.OrderEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.DTOs.OrderDtos
{
    public class OrderListDto
    {
        public int OrderId { get; set; }

        public string Address { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        // Siparişin oluşturulduğu tarih
        public decimal TotalAmount { get; set; }
        // Siparişin toplam tutarı
        public OrderStatus Status { get; set; }
        // Sipariş durumu (Hazırlanıyor, Tamamlandı, İptal)
        public bool PaymentStatus { get; set; }
        // Ödeme yapıldı mı yapılmadı mı

        public List<OrderItemListDto> OrderItems { get; set; }

        public int CustomerId { get; set; }
        // Bu sipariş hangi müşteriye ait (Foreign Key)
    }
}
