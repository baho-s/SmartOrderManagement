using MediatR;
using SmartOrderManagement.Application.DTOs.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Categories.Query.GetCategoryListTree
{
    public record GetCategoryListTreeQuery : IRequest<List<CategoryTreeDto>>
    {
    }
}
