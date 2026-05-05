using Microsoft.EntityFrameworkCore;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Domain.Enums.OrderEnums;
using SmartOrderManagement.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.AddAsync(customer);
        }

        public void Delete(Customer customer)
        {
            _context.Customers.Remove(customer);
        }

        public async Task<List<Customer>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Customers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            var value = await _context.Customers.FindAsync(id);
            return value;
        }

        public async Task<bool> OrdersActive(Customer customer)
        {
            // Müşterinin aktif siparişleri var mı kontrolü eğer varsa true yoksa false döner
            var active = await _context.Customers
               .Where(c => c.CustomerId == customer.CustomerId)
               .SelectMany(c => c.Orders)
               .AnyAsync(o => o.Status == OrderStatus.Hazirlaniyor);
            return active;
        }

        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
        }
    }
}
