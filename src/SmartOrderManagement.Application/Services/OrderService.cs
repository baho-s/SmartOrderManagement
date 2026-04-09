using AutoMapper;
using SmartOrderManagement.Application.DTOs.OrderDtos;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.Services;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var value= _mapper.Map<Order>(createOrderDto);
            await _orderRepository.AddAsync(value);
            return value.OrderId;
        }

        public async Task DeleteOrderAsync(int id)
        {
            if (id <= 0)
            {
                throw new NotFoundException("Lütfen pozitif bir Id giriniz.");
            }
            var value= await _orderRepository.GetByIdAsync(id);
            if (value is null)
            {
                throw new NotFoundException("Girilen Id'ye ait sipariş bulunamadı");
            }
            await _orderRepository.DeleteAsync(value);
        }

        public async Task<List<OrderListDto>> GetAllAsync()
        {
            var values=await _orderRepository.GetAllAsync();
            return _mapper.Map<List<OrderListDto>>(values);
        }

        public async Task<OrderByIdDto> GetByIdAsync(int id)
        {
            var value=await _orderRepository.GetByIdAsync(id);
            if (value is null)
            {
                throw new NotFoundException("Girilen Id'ye ait sipariş bulunamadı");
            }
            return _mapper.Map<OrderByIdDto>(value);
        }

        public async Task UpdateOrderAsync(int id, UpdateOrderDto updateOrderDto)
        {
            if(id!=updateOrderDto.OrderId)
            {
                throw new BusinessRuleException("Güncellemek istediğiniz siparişin Id'si ile güncelleme verisinin Id'si eşleşmelidir.");
            }
            var value=_mapper.Map<Order>(updateOrderDto);
            await _orderRepository.UpdateAsync(value);
        }
    }
}
