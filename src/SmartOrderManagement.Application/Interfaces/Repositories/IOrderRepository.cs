using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);

        IQueryable<Order> GetOrdersAsQueryable();
        Task<List<Order>> GetOrdersListAsync(int page, int pageSize);

        Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
        // Token'dan gelen CustomerId ile
        // sadece o müşteriye ait siparişleri getirecek


    }
}
