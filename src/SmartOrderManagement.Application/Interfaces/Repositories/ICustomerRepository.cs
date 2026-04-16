using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Application.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        void Update(Customer customer);
        void Delete(Customer customer);

        Task<bool> OrdersActive(Customer customer);
        Task<Customer?> GetByIdAsync(int id);

        Task<List<Customer>> GetAllAsync(int pageNumber, int pageSize);
    }
}
