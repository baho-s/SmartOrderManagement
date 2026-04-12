using MediatR;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.UpdateProductCategoryId
{
    public class UpdateProductCategoryIdCommandHandler : IRequestHandler<UpdateProductCategoryIdCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCategoryIdCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateProductCategoryIdCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if(product == null)
            {
                throw new Exception($"Bu Id'ye sahip ürün bulunamadı: {request.ProductId}");
            }
            product.UpdateProductCategoryId(request.NewCategoryId);
            await _productRepository.UpdateAsync(product);
            await _unitOfWork.CommitAsync();
        }
    }
}
