using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartOrderManagement.Application.DTOs.OrderDtos;
using SmartOrderManagement.Application.Features.Orderds.CreateOrder;
using SmartOrderManagement.Application.Features.Orderds.GetOrderById;
using SmartOrderManagement.Application.Features.Orderds.UpdateOrderStatus;
using SmartOrderManagement.Application.Interfaces.Services;

namespace SmartOrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly CreateOrderHandler _createOrderHandler;
        private readonly UpdateOrderStatusHandler _updateOrderStatusHandler;
        private readonly GetOrderByIdHandler _getOrderByIdHandler;

        public OrdersController(IOrderService orderService, CreateOrderHandler createOrderHandler, UpdateOrderStatusHandler updateOrderStatusHandler, GetOrderByIdHandler getOrderByIdHandler)
        {
            _orderService = orderService;
            _createOrderHandler = createOrderHandler;
            _updateOrderStatusHandler = updateOrderStatusHandler;
            _getOrderByIdHandler = getOrderByIdHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            int orderId = await _createOrderHandler.Handle(command);
            return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, null);
        }

        [HttpPatch("{id}/status")]//=Patch=Kısmi güncelleme işlemi için kullanılır.
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusCommand command)
        {
            command.OrderId = id; // ID'yi komut nesnesine atıyoruz//Çünkü URL'den gelen ID'yi kullanarak hangi siparişin durumunu güncelleyeceğimizi belirtiyoruz.
            await _updateOrderStatusHandler.Handle(command);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var query = new GetOrderByIdQuery { OrderId = id }; 
            var order = await _getOrderByIdHandler.Handle(query);
            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            await _orderService.UpdateOrderAsync(id, updateOrderDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}
