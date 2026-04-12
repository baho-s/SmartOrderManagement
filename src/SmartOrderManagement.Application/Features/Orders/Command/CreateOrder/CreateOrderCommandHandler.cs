using AutoMapper;
using FluentValidation;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using SmartOrderManagement.Application.Interfaces.Validators.OrderValidators;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Orders.Command.CreateOrder
{   //Handler Repository bilir
    //UnitOfWork bilir
    //Handler, Command'ı alır, doğrular, iş kurallarını uygular,
    //Repository'ye gönderir ve UnitOfWork ile işlemi tamamlar.

    //Handler DbContex,SQL,HTTP bilmez.
    public class CreateOrderCommandHandler
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICreateOrderValidator _validator;
        public CreateOrderCommandHandler(IProductRepository productRepository, IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper, ICreateOrderValidator validator)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<int> Handle(CreateOrderCommand command)
        {
            // Command doğrulama (CreateOrderValidator ile)
            // İş kurallarını uygulama
            // Repository'ye sipariş ekleme
            // UnitOfWork ile işlemi tamamlama
            //Gelen command'ı validator ile kontrol edicem.

            var validationResult = await _validator.ValidateAsync(command);
            if(!validationResult.IsValid)
            {
                throw new ValidationMyException("Validasyon hatası.");
            }

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
