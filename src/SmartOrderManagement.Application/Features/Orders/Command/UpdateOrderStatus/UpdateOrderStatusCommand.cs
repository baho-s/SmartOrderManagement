using SmartOrderManagement.Domain.Enums.OrderEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orders.Command.UpdateOrderStatus
{
    public class UpdateOrderStatusCommand
    {
        public int OrderId { get; set; }
        public OrderStatus NewStatus { get; init; }
        //Burada domaindeki OrderStatus enumunu kullanarak güncelleme yapacağız.
        //Bu durum DDD prensiplerine uygun bir yaklaşım çünkü domain katmanındaki
        //enumları kullanarak iş kurallarını koruyabiliriz. Ayrıca, bu sayede
        //geçerli durumları sınırlayarak hatalı durum güncellemelerinin önüne geçebiliriz.
    }
}
