using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SmartOrderManagement.Application.Features.Auth.Command.Login;
using SmartOrderManagement.Application.Features.Auth.Command.Register;

namespace SmartOrderManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RegisterCommandHandler _registerCommandHandler;
        private readonly LoginCommandHandler _loginCommandHandler;

        public AuthController(RegisterCommandHandler registerCommandHandler, LoginCommandHandler loginCommandHandler)
        {
            _registerCommandHandler = registerCommandHandler;
            _loginCommandHandler = loginCommandHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var token = await _registerCommandHandler.Handle(command);
            // Register olan kullanıcıya direkt token dönüyoruz
            // Böylece kayıt sonrası tekrar login olmak zorunda kalmaz
            return Ok(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var token = await _loginCommandHandler.Handle(command);
            return Ok(new { Token = token });
        }
    }
}
