using MediatR;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.UpdatePorductName
{
    public class UpdateProductNameCommandHandler : IRequestHandler<UpdateProductNameCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductNameCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateProductNameCommand request, CancellationToken cancellationToken)
        {
            var product=await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            product.UpdateProductName(request.NewProductName);
            await _productRepository.UpdateAsync(product);
            await _unitOfWork.CommitAsync();
        }
    }
}
