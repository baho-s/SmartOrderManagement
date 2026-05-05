using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Features.Categories.Command.DeleteCategoryMoveChildren
{
    public class DeleteCategoryMoveChildrenCommand:IRequest
    {
        public int CategoryId { get; set; }
        public int NewParentCategoryId { get; set; }
    }
}
