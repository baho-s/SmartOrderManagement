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
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        /* public async Task<int> CreateOrderAsync(CreateOrderDto createOrderDto)
         {


             var order = _mapper.Map<Order>(createOrderDto);
             order.TotalAmount = 0;           

             if (createOrderDto.OrderItems != null)
             {
                 foreach (var itemDto in createOrderDto.OrderItems)
                 {                    
                     var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                     if (product is null)
                     {
                         throw new NotFoundException($"Ürün Id'si {itemDto.ProductId} olan ürün bulunamadı.");
                     }

                     var orderItem = new OrderItem
                     {
                         ProductId = product.ProductId,
                         Quantity = itemDto.Quantity,
                         UnitPrice = product.ProductPrice,
                         TotalPrice = product.ProductPrice * itemDto.Quantity
                     };                   
                     order.AddOrderItem(orderItem);

                 }
             }            

             await _orderRepository.AddAsync(order);
             return order.OrderId;
         }*/

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
