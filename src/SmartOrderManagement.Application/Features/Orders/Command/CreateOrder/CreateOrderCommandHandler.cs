using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    public class CreateOrderCommandHandler:IRequestHandler<CreateOrderCommand,int>
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICreateOrderValidator _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        // IHttpContextAccessor → o anki HTTP isteğine erişmemizi sağlar
        // Token'dan CustomerId okumak için kullanacağız
        public CreateOrderCommandHandler(IProductRepository productRepository, IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper, ICreateOrderValidator validator, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> Handle(CreateOrderCommand command,CancellationToken cancellationToken)
        {
            // 1. Token'dan CustomerId oku
            var customerIdClaim = _httpContextAccessor.HttpContext?
                .User.FindFirst("CustomerId");
            // HttpContext → o anki HTTP isteği
            // User → token'dan parse edilmiş kullanıcı bilgileri
            // FindFirst("CustomerId") → token içindeki CustomerId claim'i

            if (customerIdClaim == null)
                throw new Exception("Kullanıcı girişi yapılmamış.");
            var customerId = int.Parse(customerIdClaim.Value);
            // Claim string olarak gelir, int'e çeviriyoruz

            // 2. Validasyon
            var validationResult = await _validator.ValidateAsync(command);
            if(!validationResult.IsValid)
            {
                throw new ValidationMyException("Validasyon hatası.");
            }

            // 3. Order oluştur (CustomerId token'dan geldi)
            var order = new Order(customerId, command.Address);
            // Artık kullanıcı CustomerId göndermek zorunda değil
            // Token'dan otomatik aldık 

            // 4. Ürünleri ekle ve stok düş
            foreach (var item in command.CreateOrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Ürün bulunamadı: {item.ProductId}");

                product.DecreaseStock(item.Quantity);
                order.AddOrderItem(item.ProductId, item.Quantity, product.ProductPrice);
            }
            //5. Order'ı kaydet
            await _orderRepository.AddAsync(order);
            
            await _unitOfWork.CommitAsync(); // Değişiklikleri veritabanına kaydet

            return order.OrderId; // Oluşturulan siparişin ID'si dönebilir
        }
    }
}
