using SmartOrderManagement.Application.DTOs.OrderDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Services
{
    public interface IOrderService
    {
        //Task<int> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<OrderByIdDto> GetByIdAsync(int id);
        Task<List<OrderListDto>> GetAllAsync();
        Task UpdateOrderAsync(int id, UpdateOrderDto updateOrderDto);
        Task DeleteOrderAsync(int id);
    }
}
