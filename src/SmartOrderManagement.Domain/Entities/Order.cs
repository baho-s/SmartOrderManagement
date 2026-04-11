using SmartOrderManagement.Domain.Enums.OrderEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Domain.Entities
{
    public class Order:BaseEntity
    {
        
        public int OrderId { get; set; }

        public DateTime OrderDate { get; private set; }=DateTime.Now;
        // Siparişin oluşturulduğu tarih
        public decimal TotalAmount { get;private set; }
        // Siparişin toplam tutarı
        public OrderStatus Status { get;private set; }
        // Sipariş durumu (Hazırlanıyor, Tamamlandı, İptal)
        public bool PaymentStatus { get; private set; } = false;
        // Ödeme durumu (Pending, Completed, Failed)
        public readonly List<OrderItem> _orderItems =new List<OrderItem>();
        //İç mutfak (yazma+okuma) kilitli dışarıdan erişim yok.Sadece Order sınıfı içinden yönetilecek.
        // Siparişe ait ürünler (OrderItem'lar)

        public IReadOnlyCollection<OrderItem>  OrderItems=> _orderItems;
        //IReadOnlyCollection->Sadece okunabilir bir koleksiyon.
        //_orderItems listesini dışarıya sadece okunabilir olarak sunuyoruz.
        

        public int CustomerId { get; set; }
        // Bu sipariş hangi müşteriye ait (Foreign Key)
        public virtual Customer Customer { get; set; }
        // Siparişi veren müşteri (Navigation Property)

        public void AddOrderItem(int productId, int quantity, decimal price)
        {
            var item = new OrderItem(productId, quantity, price);

            _orderItems.Add(item);

            TotalAmount += item.TotalPrice;
        }

        public Order(int customerId)
        {
            CustomerId = customerId;
            Status = OrderStatus.Hazirlaniyor;
        }
        //Neden Constructor ile yapıyoruz? Çünkü Order oluşturulurken
        //mutlaka bir müşteri ID'si olmalı. Constructor ile bu zorunluluğu sağlıyoruz.
        //İçeride Order oluşurken otomatik olarak sipariş durumunu hazırlanıyor yapıyoruz. Böylece her zaman geçerli bir durumla başlıyoruz.
        //Peki siparişi veya diğer propertyleri tekrar güncellemek istersek ne oluyor??
        //DDD prensiplerine göre, entity'lerin kendi iş kurallarını ve davranışlarını içermesi gerekir. Bu yüzden
        //sipariş durumunu güncellemek için ayrı bir method yazabiliriz. Böylece sadece geçerli durumlara izin veririz
        //ve iş kurallarını korumuş oluruz.
        //Sipariş durumunu güncellemek için ayrı bir method yazabiliriz. Örneğin:
        // Status değiştirme methodları
        public void OrderStatusTamamlandi()
        {
            if (Status != OrderStatus.Hazirlaniyor)
                throw new Exception("Sipariş zaten hazırlanıyor!");//Kendi Exception'ımızı fırlatmak istersek Şuanda Applicationdaki BusinessRuleException'u buraya taşımalıyız.

            Status = OrderStatus.Tamamlandi;
        }

        public void OrderStatusIptal()
        {
            if (Status == OrderStatus.Tamamlandi)
                throw new Exception("Sipariş zaten tamamlandı!");//Kendi Exception'ımızı fırlatmak istersek Şuanda Applicationdaki BusinessRuleException'u buraya taşımalıyız.
            Status = OrderStatus.Iptal;
        }


        public void OdemeYap()
        {
            if(PaymentStatus)
                throw new Exception("Ödeme zaten yapılmış!");//Kendi Exception'ımızı fırlatmak istersek Şuanda Applicationdaki BusinessRuleException'u buraya taşımalıyız.
            PaymentStatus = true;
        }

        public List<OrderItem> OrderItemsList(Order order)
        {
            var list = new List<OrderItem>();
            foreach (var item in order.OrderItems)
            {
                list.Add(item);
            }   
            return list;
        }
    }
}
