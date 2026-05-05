using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Features.Categories.Command.DeleteCategory
{
    public class DeleteCategoryItsChildrenCommand:IRequest
    {//Kategori ve tüm alt kategorilerini sileceğiz.
        public int CategoryId { get; set; }
    }
}
