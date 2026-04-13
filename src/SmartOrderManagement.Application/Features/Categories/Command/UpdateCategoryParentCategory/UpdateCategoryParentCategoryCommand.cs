using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Categories.Command.UpdateCategoryParentCategory
{
    public class UpdateCategoryParentCategoryCommand:IRequest
    {
        public int CategoryId { get; set; }
        public int? newParentCategoryId { get; init; }
    }
}
