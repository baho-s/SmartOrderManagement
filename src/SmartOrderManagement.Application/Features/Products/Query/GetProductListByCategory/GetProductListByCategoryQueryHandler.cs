using AutoMapper;
using MediatR;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using SmartOrderManagement.Application.Exceptions;
using SmartOrderManagement.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Features.Products.Query.GetProductListByCategory
{
    public class GetProductListByCategoryQueryHandler:IRequestHandler<GetProductListByCategoryQuery, List<ProductListDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductListByCategoryQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductListDto>> Handle(GetProductListByCategoryQuery request, CancellationToken cancellationToken)
        {
            var values = await _productRepository.GetProductListByIdCategory(request.CategoryId);
            
            if(values.Count == 0)
            {
                throw new NotFoundException($"Bu Kategori Id'ye sahip kategori bulunamadı.{request.CategoryId}");
            }

            if (!values.Any())
            {
                throw new NotFoundException($"Bu Kategori Id'ye sahip ürün bulunamadı.{request.CategoryId}");
            }

            return _mapper.Map<List<ProductListDto>>(values);
        }
    }
}
