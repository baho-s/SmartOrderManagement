using MediatR;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Products.Query.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductByIdDto>
    {
        public int ProductId { get; set; }
    }
}
