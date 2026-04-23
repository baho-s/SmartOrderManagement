using SmartOrderManagement.Application.DTOs.OrderItemDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.DTOs.OrderDtos
{
    public class GetMyOrdersDto
    {
        public int OrderId { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool PaymentStatus { get; set; }
        public List<GetMyOrderItemDto> OrderItems { get; set; } = new();
    }
}
