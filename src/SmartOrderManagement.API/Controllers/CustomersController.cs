using AutoMapper.Configuration.Annotations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartOrderManagement.Application.DTOs.CustomerDtos;
using SmartOrderManagement.Application.Features.Customers.Command.CreateCustomer;
using SmartOrderManagement.Application.Features.Customers.Command.DeleteCustomer;
using SmartOrderManagement.Application.Features.Customers.Command.UpdateCustomer;
using SmartOrderManagement.Application.Features.Customers.Command.UpdateCustomerAddress;
using SmartOrderManagement.Application.Features.Customers.Command.UpdateCustomerEmail;
using SmartOrderManagement.Application.Features.Customers.Command.UpdateCustomerFullName;
using SmartOrderManagement.Application.Features.Customers.Query.GetCustomerById;
using SmartOrderManagement.Application.Features.Customers.Query.GetCustomerList;
using SmartOrderManagement.Application.Interfaces.Services;
using System.Threading.Tasks;

namespace SmartOrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMediator _mediator;

        public CustomersController(ICustomerService customerService, IMediator mediator)
        {
            _customerService = customerService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync([FromQuery] GetCustomerListQuery query)
        {
            var values = await _mediator.Send(query);
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateCustomerCommand command)
        {
            var id = await _mediator.Send(command);
            return Created($"api/customers/{command.FullName}", id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var query = new GetCustomerByIdQuery { CustomerId = id };
            var value = await _mediator.Send(query);    
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            var command = new DeleteCustomerCommand { CustomerId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("{id}/address")]
        public async Task<IActionResult> UpdateCustomerAddressAsync(int id, [FromBody] UpdateCustomerAddressCommand command)
        {
            command.CustomerId = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("{id}/email-phone")]
        public async Task<IActionResult> UpdateCustomerEmailAndPhoneAsync(int id, [FromBody] UpdateCustomerEmailPhoneCommand command)
        {
            command.CustomerId = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("{id}/fullname")]
        public async Task<IActionResult> UpdateCustomerFullNameAsync(int id, [FromBody] UpdateCustomerFullNameCommand command)
        {
            command.CustomerId = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAsync(int id, [FromBody] UpdateCustomerCommand command)
        {
            command.CustomerId = id;
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
