using MediatR;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using SmartOrderManagement.Application.Interfaces.Validators.ProductValidators;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Command.CreateProduct
{
    public class CreateProductCommandHandler:IRequestHandler<CreateProductCommand,int>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICreateProductValidator _createProductValidator;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, ICreateProductValidator createProductValidator)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _createProductValidator = createProductValidator;
        }

        public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var validationResult = await _createProductValidator.ValidateAsync(command);
            if (validationResult.IsValid)
            {
                var product = new Product(command.ProductName, command.ProductPrice, command.ProductStock, command.IsActive, command.CategoryId);
                await _productRepository.AddAsync(product);
                await _unitOfWork.CommitAsync();
                return product.ProductId;
            }
            else if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ValidationMyException(errors);
            }
            else
            {
                throw new Exception("Bilinmeyen bir hata oluştu.");
            }
        }
    }
}
