using SmartOrderManagement.Application.DTOs.OrderItemDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Services
{
    public interface IOrderItemService
    {
        Task<int> AddOrderItemAsync(CreateOrderItemDto createOrderItemDto);
        Task UpdateOrderItemAsync(int id, UpdateOrderItemDto updateOrderItemDto);
        Task DeleteOrderItemAsync(int id);
        Task<OrderItemByIdDto> GetOrderItemByIdAsync(int id);
        Task<IEnumerable<OrderItemListDto>> GetOrderItemsAsync();
    }
}
