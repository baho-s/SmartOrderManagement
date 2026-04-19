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
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }        

        public async Task Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(command.ProductId);
            if (product is null)
            {
                throw new NotFoundException("Bu Id'ye ait ürün bulunamadı.");
            }
            await _productRepository.DeleteAsync(product);
            await _unitOfWork.CommitAsync();
        }
    }
}
