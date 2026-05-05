using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Auth.Command.Register
{
    public class RegisterCommand
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        // Kullanıcıdan alacağımız bilgiler
        // Bu bilgilerle hem AppUser hem Customer oluşturacağız
    }
}
