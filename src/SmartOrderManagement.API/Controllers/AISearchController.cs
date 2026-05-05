using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartOrderManagement.Application.Interfaces.AI;

namespace SmartOrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AISearchController : ControllerBase
    {
        private readonly IRagService _ragService;

        public AISearchController(IRagService ragService)
        {
            _ragService = ragService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string question)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return BadRequest("Question parameter is required.");
            }
            var response = await _ragService.GetAugmentedPromptAsync(question);
            return Ok(new { Answer = response });
        }
    }
}
