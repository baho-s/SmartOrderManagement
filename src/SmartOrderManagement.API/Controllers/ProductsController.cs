using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartOrderManagement.Application.DTOs.ProductDtos;
using SmartOrderManagement.Application.Features.Orders.Command.UpdateOrderStatus;
using SmartOrderManagement.Application.Features.Products.Command.CreateProduct;
using SmartOrderManagement.Application.Features.Products.Command.DeleteProduct;
using SmartOrderManagement.Application.Features.Products.Command.UpdateProduct;
using SmartOrderManagement.Application.Features.Products.Command.UpdateProductCategoryId;
using SmartOrderManagement.Application.Features.Products.Command.UpdateProductIsActive;
using SmartOrderManagement.Application.Features.Products.Query.GetProductById;
using SmartOrderManagement.Application.Features.Products.Query.GetProductList;
using SmartOrderManagement.Application.Interfaces.Services;

namespace SmartOrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        //private readonly CreateProductCommandHandler _createProductCommandHandler;
        //private readonly DeleteProductCommandHandler _deleteProductCommandHandler;
        private readonly IMediator _mediator;

        public ProductsController(IProductService productService, IMediator mediator)
        {
            _productService = productService;
            _mediator = mediator;
            //_createProductCommandHandler = createProductCommandHandler;
            //_deleteProductCommandHandler = deleteProductCommandHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] GetProductListQuery query)
        {
            var values = await _mediator.Send(query);
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Created($"api/products/{command.ProductName}", result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var command = new DeleteProductCommand { ProductId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var command = new GetProductByIdQuery { ProductId = id };
            var value = await _mediator.Send(command);
            return Ok(value);
        }

        [HttpPut("{id}")]
        public async  Task<IActionResult> UpdateProduct(int id,[FromBody]UpdateProductCommand command)
        {
            command.ProductId = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("{id}/isActive")]//=Patch=Kısmi güncelleme işlemi için kullanılır.
        public async Task<IActionResult> UpdateProductIsActive(int id, [FromBody] UpdateProductIsActiveCommand command)
        {
            command.ProductId = id; // ID'yi komut nesnesine atıyoruz//Çünkü URL'den gelen ID'yi kullanarak hangi ürünün durumunu güncelleyeceğimizi belirtiyoruz.
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("{id}/categoryId")]//=Patch=Kısmi güncelleme işlemi için kullanılır.
        public async Task<IActionResult> UpdateProductCategoryId(int id, [FromBody] UpdateProductCategoryIdCommand command)
        {
            command.ProductId = id; // ID'yi komut nesnesine atıyoruz//Çünkü URL'den gelen ID'yi kullanarak hangi ürünün durumunu güncelleyeceğimizi belirtiyoruz.
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
