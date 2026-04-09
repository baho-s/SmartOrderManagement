using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Domain.Entities
{
    public class Customer:BaseEntity
    {
        public int CustomerId { get; set; }

        public string FullName { get; set; }=string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public virtual ICollection<Order> Orders { get; set; }=new HashSet<Order>();
        // Müşteri birden fazla sipariş verebilir
    }
}
