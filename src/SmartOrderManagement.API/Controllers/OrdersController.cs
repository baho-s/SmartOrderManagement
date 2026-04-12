using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartOrderManagement.Application.DTOs.OrderDtos;
using SmartOrderManagement.Application.Features.Orders.Command.CreateOrder;
using SmartOrderManagement.Application.Features.Orders.Command.DeleteOrder;
using SmartOrderManagement.Application.Features.Orders.Command.UpdateOrderAddress;
using SmartOrderManagement.Application.Features.Orders.Command.UpdateOrderStatus;
using SmartOrderManagement.Application.Features.Orders.Command.UpdateOrderTotalAmount;
using SmartOrderManagement.Application.Features.Orders.Query.GetOrderById;
using SmartOrderManagement.Application.Features.Orders.Query.GetOrderList;
using SmartOrderManagement.Application.Interfaces.Services;

namespace SmartOrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly CreateOrderCommandHandler _createOrderHandler;
        private readonly UpdateOrderStatusCommandHandler _updateOrderStatusHandler;
        private readonly GetOrderByIdQueryHandler _getOrderByIdHandler;
        private readonly GetOrdersListQueryHandler _getOrdersListQueryHandler;
        private readonly UpdateOrderAddressCommandHandler _updateOrderAddressCommandHandler;
        private readonly UpdateOrderTotalAmountCommandHandler _updateOrderTotalAmountCommandHandler;
        private readonly DeleteOrderCommandHandler _deleteOrderCommandHandler;

        public OrdersController(IOrderService orderService, CreateOrderCommandHandler createOrderHandler, UpdateOrderStatusCommandHandler updateOrderStatusHandler, GetOrderByIdQueryHandler getOrderByIdHandler, GetOrdersListQueryHandler getOrdersListQueryHandler, UpdateOrderAddressCommandHandler updateOrderAddressCommandHandler, UpdateOrderTotalAmountCommandHandler updateOrderTotalAmountCommandHandler, DeleteOrderCommandHandler deleteOrderCommandHandler)
        {
            _orderService = orderService;
            _createOrderHandler = createOrderHandler;
            _updateOrderStatusHandler = updateOrderStatusHandler;
            _getOrderByIdHandler = getOrderByIdHandler;
            _getOrdersListQueryHandler = getOrdersListQueryHandler;
            _updateOrderAddressCommandHandler = updateOrderAddressCommandHandler;
            _updateOrderTotalAmountCommandHandler = updateOrderTotalAmountCommandHandler;
            _deleteOrderCommandHandler = deleteOrderCommandHandler;
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

        [HttpPatch("{id}/address")]//=Patch=Kısmi güncelleme işlemi için kullanılır.
        public async Task<IActionResult> UpdateOrderAddress(int id, [FromBody] UpdateOrderAddressCommand command)
        {
            command.OrderId = id; // ID'yi komut nesnesine atıyoruz//Çünkü URL'den gelen ID'yi kullanarak hangi siparişin adresini güncelleyeceğimizi belirtiyoruz.
            await _updateOrderAddressCommandHandler.Handle(command);
            return NoContent();
        }

        [HttpPatch("{id}/totalamount")]//=Patch=Kısmi güncelleme işlemi için kullanılır.
        public async Task<IActionResult> UpdateOrderTotalAmount(int id, [FromBody] UpdateOrderTotalAmountCommand command)
        {
            //indirim kuponu sonrası toplam tutar güncellemesi gibi senaryolarda bu endpoint'i kullanabiliriz.
            command.OrderId = id; // ID'yi komut nesnesine atıyoruz//Çünkü URL'den gelen ID'yi kullanarak hangi siparişin toplam tutarını güncelleyeceğimizi belirtiyoruz.
            await _updateOrderTotalAmountCommandHandler.Handle(command);
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
        public async Task<IActionResult> GetOrders([FromQuery] GetOrdersListQuery query)
        {
            var orders = await _getOrdersListQueryHandler.Handle(query); //Örnek olarak sayfa numarası ve sayfa boyutu veriyoruz
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
            var command= new DeleteOrderCommand { OrderId = id };
            await _deleteOrderCommandHandler.Handle(command);
            return NoContent();
        }
    }
}
