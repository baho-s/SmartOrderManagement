using MediatR;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Features.Categories.Command.DeleteCategory
{
    public class DeleteCategoryItsChildrenCommandHandler:IRequestHandler<DeleteCategoryItsChildrenCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unifOfWork;

        public DeleteCategoryItsChildrenCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unifOfWork)
        {
            _categoryRepository = categoryRepository;
            _unifOfWork = unifOfWork;
        }

        public async Task Handle(DeleteCategoryItsChildrenCommand command, CancellationToken cancellationToken)
        {
            var rootCategory=await _categoryRepository.GetCategoryWithSubAsync(command.CategoryId);
            if(rootCategory == null)
            {
                throw new Exception($"Girilen Id'ye ait kategori bulunamadı: {command.CategoryId}");
            }
            rootCategory.DeleteCategory();
            foreach (var subCategory in rootCategory.SubCategories)
            {
                subCategory.DeleteCategory();
            }
            await _unifOfWork.CommitAsync();
        }
    }
}
