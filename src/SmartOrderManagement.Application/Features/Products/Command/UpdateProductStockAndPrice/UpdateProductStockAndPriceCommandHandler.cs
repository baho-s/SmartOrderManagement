using MediatR;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.UpdateProductStockAndPrice
{
    public class UpdateProductStockAndPriceCommandHandler : IRequestHandler<UpdateProductStockAndPriceCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateProductStockAndPriceCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task Handle(UpdateProductStockAndPriceCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new ArgumentException("Product not found.");
            }

            product.UpdateProductStockAndPrice(request.NewPrice, request.NewStock);
            await _productRepository.UpdateAsync(product);
            await _unitOfWork.CommitAsync();
        }
    }
}
