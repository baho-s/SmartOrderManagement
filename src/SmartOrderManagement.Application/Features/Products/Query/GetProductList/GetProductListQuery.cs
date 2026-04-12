using MediatR;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Query.GetProductList
{
    public class GetProductListQuery:IRequest<List<ProductListDto>>
    {
        public byte PageNumber { get; set; } = 1;
        public byte PageSize { get; set; } = 10;
    }
}
