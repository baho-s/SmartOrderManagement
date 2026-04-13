using AutoMapper;
using MediatR;
using SmartOrderManagement.Application.DTOs.CategoryDtos;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Categories.Query.GetCategoryListTree
{
    public class GetCategoryListTreeQueryHandler : IRequestHandler<GetCategoryListTreeQuery, List<CategoryTreeDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryListTreeQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoryTreeDto>> Handle(GetCategoryListTreeQuery request, CancellationToken cancellationToken)
        {
            //GetAllCategoriesForTreeAsync metodu: Ağaç yapısı için tüm kategorileri ve
            //alt kategorilerini içeren bir liste döndürür.
            var categories = await _categoryRepository.GetAllCategoriesForTreeAsync();

            // Root kategorileri bulmak için ParentCategoryId'si null olanları filtreliyoruz.
            var rootCategories = categories.Where(c => c.ParentCategoryId == null).ToList();

            var treeDtos = new List<CategoryTreeDto>();
            foreach (var root in rootCategories)
            {
                var treeDto = BuildCategoryTree(root, categories);
                treeDtos.Add(treeDto);
            }
            return treeDtos;
        }

        private CategoryTreeDto BuildCategoryTree(Category category, List<Category> allCategories)
        {
            var treeDto = _mapper.Map<CategoryTreeDto>(category);
            var childCategories = allCategories.Where(c => c.ParentCategoryId == category.CategoryId).ToList();
            foreach (var child in childCategories)
            {
                var childTreeDto = BuildCategoryTree(child, allCategories);
                treeDto.SubCategories.Add(childTreeDto);
            }
            return treeDto;
        }

    }
}
