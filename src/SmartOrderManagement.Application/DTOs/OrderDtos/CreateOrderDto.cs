using SmartOrderManagement.Application.DTOs.OrderItemDtos;
using SmartOrderManagement.Domain.Enums.OrderEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.DTOs.OrderDtos
{
    public class CreateOrderDto
    {
        public decimal TotalAmount { get; set; }
        // Siparişin toplam tutarı
        public OrderStatus Status { get; set; }
        // Sipariş durumu (Hazırlanıyor, Tamamlandı, İptal)
        public bool PaymentStatus { get; set; }
        // Ödeme yapıldı mı yapılmadı mı

        public int CustomerId { get; set; }
        // Bu sipariş hangi müşteriye ait (Foreign Key)

        //Yeni siparişler oluşturulurken, CreateOrderDto içinde OrderItemDto listesi bulunmaz.
        //Bunun yerine, CreateOrderItemDto listesi bulunur. Çünkü OrderItem nesneleri,
        //Order nesnesinin bir parçasıdır ve Aggregate Root olarak Order tarafından yönetilir.
        //Bu nedenle, CreateOrderDto içinde sadece CreateOrderItemDto listesi bulunur ve bu liste,
        //sipariş oluşturulurken doldurulmalıdır.
        public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();
        // Siparişe ait ürünler (OrderItemDto listesi),
        //Bu liste, sipariş oluşturulurken CreateOrderItemDto nesnelerini içerecek şekilde doldurulmalıdır.
        //Ancak Aggregate Root olarak üretilen Order nesnesi, OrderItem nesnelerini yönetir ve bu nedenle CreateOrderDto içinde sadece CreateOrderItemDto listesi bulunur.
    }
}
