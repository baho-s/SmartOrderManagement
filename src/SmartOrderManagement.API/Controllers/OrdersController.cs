using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartOrderManagement.Application.Features.Orders.Command.CreateOrder;
using SmartOrderManagement.Application.Features.Orders.Command.DeleteOrder;
using SmartOrderManagement.Application.Features.Orders.Command.UpdateOrderAddress;
using SmartOrderManagement.Application.Features.Orders.Command.UpdateOrderStatus;
using SmartOrderManagement.Application.Features.Orders.Command.UpdateOrderTotalAmount;
using SmartOrderManagement.Application.Features.Orders.Query.GetMyOrders;
using SmartOrderManagement.Application.Features.Orders.Query.GetOrderById;
using SmartOrderManagement.Application.Features.Orders.Query.GetOrderList;
using SmartOrderManagement.Application.Interfaces.Services;

namespace SmartOrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;        

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        // Sadece giriş yapmış kullanıcılar sipariş verebilir
        // Token olmadan istek gelirse 401 döner
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            int orderId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, null);
            
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusCommand command)
        {
            command.OrderId = id; 
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("{id}/address")]
        public async Task<IActionResult> UpdateOrderAddress(int id, [FromBody] UpdateOrderAddressCommand command)
        {
            command.OrderId = id; 
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("{id}/totalamount")]
        public async Task<IActionResult> UpdateOrderTotalAmount(int id, [FromBody] UpdateOrderTotalAmountCommand command)
        {
            //indirim kuponu sonrası toplam tutar güncellemesi gibi senaryolarda bu endpoint'i kullanabiliriz.
            command.OrderId = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var query = new GetOrderByIdQuery { OrderId = id }; 
            var order = await _mediator.Send(query);
            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] GetOrdersListQuery query)
        {
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }
        

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var command= new DeleteOrderCommand { OrderId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("my-orders")]
        [Authorize]        
        public async Task<IActionResult> GetMyOrders()
        {
            var orders = await _mediator.Send(new GetMyOrdersQuery());
            return Ok(orders);
        }
    }
}
