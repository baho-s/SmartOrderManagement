using Microsoft.EntityFrameworkCore;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Infrastructure.Context;

namespace SmartOrderManagement.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public void Delete(Order order)
        {
            order.IsDeleted = true;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            IEnumerable<Order> orders = await _context.Orders.ToListAsync();
            return orders;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            var order=await _context.Orders
                .Include(o=>o.OrderItems)
                .FirstOrDefaultAsync(o=>o.OrderId==id);
            return order;
        }

        public IQueryable<Order> GetOrdersAsQueryable()
        {
            //Bu metod async olmak zorunda değil çünkü IQueryable döndük.
            //Bu veritabanına sorgu atılmadı henüz.//Sorgu oluşturuldu ama çalıştırılmadı.
            return _context.Orders.AsQueryable();
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _context.Orders
                    .Where(o => o.CustomerId == customerId)
                    // Sadece o müşteriye ait siparişler
                    .Include(o => o.OrderItems)
                    // Sipariş kalemleri de gelsin
                    .OrderByDescending(o => o.OrderDate)
                    // En yeni sipariş en üstte
                    .ToListAsync();
        }

        public async Task<List<Order>> GetOrdersListAsync(int page, int pageSize)
        {
            var orders=await _context.Orders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x=>x.OrderItems)
                .ToListAsync();
            return orders;
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
        }
    }
}
