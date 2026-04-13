using MediatR;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Categories.Command.UpdateCategoryParentCategory
{
    public class UpdateCategoryParentCategoryCommandHandler : IRequestHandler<UpdateCategoryParentCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryParentCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateCategoryParentCategoryCommand request, CancellationToken cancellationToken)
        {
            
            var category =await  _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new Exception($"Bu Id'ye sahip kategori bulunamadı: {request.CategoryId}");
            }
            category.UpdateCategoryParentId(request.newParentCategoryId);
            _categoryRepository.Update(category);
            await _unitOfWork.CommitAsync();
        }
    }
}
