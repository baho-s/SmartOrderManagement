using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Categories.Command.UpdateCategory
{
    public class UpdateCategoryNameAndDescriptionCommand:IRequest
    {
        public int CategoryId { get; set; }
        public string newCategoryName { get; init; }
        public string newCategoryDescription { get; init; }
    }
}
