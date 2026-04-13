using MediatR;
using SmartOrderManagement.Application.DTOs.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Categories.Query.GetCategoryById
{
    public class GetCategoryByIdQuery: IRequest<CategoryByIdDto>
    {
        public int CategoryId { get; set; }
    }
}
