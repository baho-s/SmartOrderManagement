using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.DTOs.OrderItemDtos
{
    public class OrderItemByIdDto
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

        public int ProductId { get; set; }
        // Bu sipariş satırı hangi ürüne ait (Foreign Key)
    }
}
