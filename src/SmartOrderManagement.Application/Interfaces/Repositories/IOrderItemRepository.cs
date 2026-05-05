using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Repositories
{
    public interface IOrderItemRepository
    {
        Task AddAsync(OrderItem orderItem);
        Task<OrderItem> GetByIdAsync(int id);
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task UpdateAsync(OrderItem orderItem);
        void Delete(OrderItem orderItem);
    }
}
