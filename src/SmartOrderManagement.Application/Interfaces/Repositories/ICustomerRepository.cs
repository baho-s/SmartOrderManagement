using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Application.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Customer customer);
        Task<Customer?> GetByIdAsync(int id);

        Task<IEnumerable<Customer>> GetAllAsync();
    }
}
