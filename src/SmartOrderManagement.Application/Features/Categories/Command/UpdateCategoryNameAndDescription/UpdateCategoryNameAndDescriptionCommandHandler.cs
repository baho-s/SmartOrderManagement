using MediatR;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Categories.Command.UpdateCategory
{
    public class UpdateCategoryNameAndDescriptionCommandHandler : IRequestHandler<UpdateCategoryNameAndDescriptionCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryNameAndDescriptionCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateCategoryNameAndDescriptionCommand request, CancellationToken cancellationToken)
        {
            if (request.CategoryId <= 0)
            {
                throw new NotFoundException($"Id pozitif olmalı.:{request.CategoryId}");
            }
            var category=await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new NotFoundException($"Bu Id'ye ait Kategori bulunamadı: {request.CategoryId} ");
            }
            category.UpdateCategoryNameAndDescription(request.newCategoryName,request.newCategoryDescription);
            _categoryRepository.Update(category);
            await _unitOfWork.CommitAsync();
        }
    }
}
