using MediatR;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product=await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new Exception($"Id'ye ait ürün bulunumadı: {request.ProductId}"); 
            }
            product.UpdateProductName(request.NewProductName);
            product.UpdateProductStockAndPrice(request.NewProductPrice, request.NewProductStock);
            product.UpdateProductIsActive(request.NewIsActive);
            product.UpdateProductCategoryId(request.NewCategoryId);
            await _productRepository.UpdateAsync(product);
            await _unitOfWork.CommitAsync();
        }
    }
}
