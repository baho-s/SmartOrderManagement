using MediatR;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.DeleteProduct
{
    public class DeleteProductCommandHandler:IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            /*var product = await _productRepository.GetByIdAsync(command.ProductId);
            if (product is null)
            {
                throw new NotFoundException("Bu Id'ye ait ürün bulunamadı.");
            }
            
            await _productRepository.DeleteAsync(product);
            await _unitOfWork.CommitAsync();*/

            var product = await _productRepository.DeleteProductNew(command.ProductId);            

            // 1. Ürün bulundu mu?
            if (product != null)
            {

                // 2. OrderItems var mı
                var orderItems = product.OrderItems.ToList();
                foreach(var orderItem in orderItems)
                {
                    // OrderItem'ı sil
                     _orderItemRepository.Delete(orderItem);
                }

                // Select(x => x.Order) ile Order listesine ulaşıyoruz
                var orders = orderItems
                    .Select(x => x.Order)
                    .Where(o => o != null);// null olan orderları ayıkla
                if (orders.Any()) // orders.Count > 0 ile aynıdır, daha moderndir
                {
                    // Silme işlemleri
                    foreach (var order in orders)
                    {
                        _orderRepository.Delete(order);                        
                    }
                }

                // Ürünü sil
                 _productRepository.Delete(product);
            }
            await _unitOfWork.CommitAsync();
        }
    }
}
