using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.DTOs.OrderItemDtos
{
    public class CreateOrderItemDto
    {

        public int Quantity { get; set; }
        // Bu üründen kaç adet sipariş verildi
        public decimal UnitPrice { get; set; }
        // Ürünün sipariş anındaki birim fiyatı
        public decimal TotalPrice { get; set; }
        // Bu satırın toplam fiyatı (Quantity * UnitPrice)
        public int ProductId { get; set; }
        // Bu sipariş satırı hangi ürüne ait (Foreign Key)
    }
}
