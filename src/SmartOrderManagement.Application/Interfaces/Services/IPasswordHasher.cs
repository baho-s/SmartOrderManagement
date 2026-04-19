using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        // Düz şifreyi alır → hash'lenmiş şifre döner

        bool VerifyPassword(string password, string passwordHash);
        // Düz şifre + hash alır → doğruysa true döner
    }
}
