using MediatR;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using SmartOrderManagement.Application.Interfaces.Validators.CategoryValidators;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Categories.Command.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICreateCategoryValidator _validator;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, ICreateCategoryValidator validator)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            //Validasyon Kontrolü
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errorMessage=validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ValidationMyException(errorMessage);
            }

            var category = new Category(request.CategoryName, request.CategoryDescription, request.ImageUrl, request.IsActive, request.ParentCategoryId);
            await _categoryRepository.AddAsync(category);
            await _unitOfWork.CommitAsync();
            return category.CategoryId;
        }
    }
}
