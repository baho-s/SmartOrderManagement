using SmartOrderManagement.Application.DTOs.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<int> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        Task UpdateCustomerAsync(int id,UpdateCustomerDto updateCustomerDto);
        Task DeleteCustomerAsync(int id);
        Task<List<CustomerListDto>> GetCustomersAsync();
        Task<CustomerByIdDto> GetByIdAsync(int id);
    }
}
