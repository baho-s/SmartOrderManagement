using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartOrderManagement.Application.DTOs.CustomerDtos;
using SmartOrderManagement.Application.Interfaces.Services;
using System.Threading.Tasks;

namespace SmartOrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var values = await _customerService.GetCustomersAsync();
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync([FromBody]CreateCustomerDto createCustomerDto)
        {
            var id = await _customerService.CreateCustomerAsync(createCustomerDto);
            return Created($"api/customers/{createCustomerDto.FullName}", id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var value = await _customerService.GetByIdAsync(id);
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto)
        {
            await _customerService.UpdateCustomerAsync(id, updateCustomerDto);
            return Ok();
        }
    }
}
