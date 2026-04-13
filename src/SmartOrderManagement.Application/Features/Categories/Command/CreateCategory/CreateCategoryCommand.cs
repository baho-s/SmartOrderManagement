using MediatR;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Categories.Command.CreateCategory
{
    public class CreateCategoryCommand:IRequest<int>
    {
        public string CategoryName { get; set; } = string.Empty;

        public string CategoryDescription { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;


        // Üst kategori FK — root ise null
        public int? ParentCategoryId { get; set; }

    }
}
