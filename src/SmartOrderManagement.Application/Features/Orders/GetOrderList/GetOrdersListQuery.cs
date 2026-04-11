using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orders.GetOrderList
{
    public class GetOrdersListQuery
    {
        public int Page { get; set; } = 1;//Sayfa numarası
        public int PageSize { get; set; } = 10;//Sayfa başına düşen kayıt sayısı
    }
}
