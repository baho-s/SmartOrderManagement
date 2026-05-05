using Microsoft.AspNetCore.Identity;
using SmartOrderManagement.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
            // BCrypt ile şifreyi hash'liyoruz
            // Infrastructure katmanında olduğu için
            // BCrypt'e erişebiliyoruz
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
            // Kullanıcının girdiği şifre ile
            // veritabanındaki hash'i karşılaştırıyoruz
        }
    }
}
