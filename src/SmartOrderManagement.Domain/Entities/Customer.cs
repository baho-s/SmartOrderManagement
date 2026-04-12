using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Domain.Entities
{
    public class Customer:BaseEntity
    {
        public int CustomerId { get; private set; }

        public string FullName { get; private set; }=string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;

        public virtual ICollection<Order> Orders { get; set; }=new HashSet<Order>();
        // Müşteri birden fazla sipariş verebilir
        public Customer(string fullName, string phone, string email, string address)
        {
            FullName = fullName;
            Phone = phone;
            Email = email;
            Address = address;
        }

        public void UpdateCustomerAddress(string newAddress)
        {
            Address = newAddress;
        }
        
        public void UpdateCustomerEmailAndPhone(string newEmail, string newPhone)
        {
            Email = newEmail;
            Phone = newPhone;
        }

        public void UpdateCustomerFullName(string newFullName)
        {
            FullName = newFullName;
        }
    }
}
