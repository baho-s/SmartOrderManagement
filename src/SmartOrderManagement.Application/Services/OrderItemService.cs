using AutoMapper;
using SmartOrderManagement.Application.DTOs.OrderItemDtos;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.Services;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;

        public OrderItemService(IOrderItemRepository orderItemRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task<int> AddOrderItemAsync(CreateOrderItemDto createOrderItemDto)
        {
            var value=_mapper.Map<OrderItem>(createOrderItemDto);
            await _orderItemRepository.AddAsync(value);
            return value.OrderItemId;
        }

        public async Task DeleteOrderItemAsync(int id)
        {
            if (id <= 0)
            {
                throw new NotFoundException($"Id pozitif olmalı.");
            }
            var value= await _orderItemRepository.GetByIdAsync(id);
            await _orderItemRepository.DeleteAsync(value);
        }

        public async Task<OrderItemByIdDto> GetOrderItemByIdAsync(int id)
        {
            var value = await _orderItemRepository.GetByIdAsync(id);
            if (value is null )
            {
                throw new NotFoundException($"Girilen Id'ye ait değer bulunamadı.");
            }
            return _mapper.Map<OrderItemByIdDto>(value);
        }

        public async Task<IEnumerable<OrderItemListDto>> GetOrderItemsAsync()
        {
            var values=await _orderItemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderItemListDto>>(values);
        }

        public async Task UpdateOrderItemAsync(int id, UpdateOrderItemDto updateOrderItemDto)
        {
            if(id <= 0)
            {
                throw new NotFoundException($"Id pozitif olmalı.");
            }
            if (id != updateOrderItemDto.OrderId)
            {
                throw new NotFoundException($"Id'ler eşleşmiyor.");
            }
            var value= _mapper.Map<OrderItem>(updateOrderItemDto);
            await _orderItemRepository.UpdateAsync(value);
        }
    }
}
