using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Auth.Command.Login
{
    public class LoginCommand
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        // Login için sadece email ve şifre yeterli
    }
}
