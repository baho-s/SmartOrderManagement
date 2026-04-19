using MediatR;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Features.Categories.Command.DeleteCategoryMoveChildren
{
    public class DeleteCategoryMoveChildrenCommandHandler : IRequestHandler<DeleteCategoryMoveChildrenCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryMoveChildrenCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteCategoryMoveChildrenCommand command, CancellationToken cancellationToken)
        {
            var deletedCategory = await _categoryRepository.GetCategoryWithSubAsync(command.CategoryId);
            var newParentCategory = await _categoryRepository.GetByIdAsync(command.NewParentCategoryId);
            if (deletedCategory == null)
            {
                throw new Exception($"Kategoriye ait Id bulunamadı: {command.CategoryId}");
            }
            if (newParentCategory == null)
            {
                throw new Exception($"Yeni üst kategoriye ait Id bulunamadı: {command.NewParentCategoryId}");
            }
            var subCategories=deletedCategory.SubCategories.ToList();
            foreach (var childCategory in subCategories)
            {
                childCategory.MoveChildCategory(command.NewParentCategoryId);
            }
            deletedCategory.DeleteCategory();
            await _unitOfWork.CommitAsync();

        }
    }
}
