using AutoMapper;
using FluentValidation;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orderds.CreateOrder
{   //Handler Repository bilir
    //UnitOfWork bilir
    //Handler, Command'ı alır, doğrular, iş kurallarını uygular,
    //Repository'ye gönderir ve UnitOfWork ile işlemi tamamlar.

    //Handler DbContex,SQL,HTTP bilmez.
    public class CreateOrderHandler
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateOrderHandler(IProductRepository productRepository, IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateOrderCommand command)
        {   
            // Command doğrulama (CreateOrderValidator ile)
            // İş kurallarını uygulama
            // Repository'ye sipariş ekleme
            // UnitOfWork ile işlemi tamamlama
            var order=_mapper.Map<Order>(command);

            foreach (var item in command.CreateOrderItems)
            {
                var product=await _productRepository.GetByIdAsync(item.ProductId);
                product.DecreaseStock(item.Quantity);
                order.AddOrderItem(item.ProductId, item.Quantity, product.ProductPrice);
            }
            await _orderRepository.AddAsync(order);
            
            await _unitOfWork.CommitAsync(); // Değişiklikleri veritabanına kaydet

            return order.OrderId; // Oluşturulan siparişin ID'si dönebilir
        }
    }
}
