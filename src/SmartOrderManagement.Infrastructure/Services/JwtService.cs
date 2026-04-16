using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartOrderManagement.Application.Interfaces.Services;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartOrderManagement.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        // appsettings.json'daki JWT ayarlarını okuyacağız

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;            
        }

        public string GenerateToken(AppUser appUser)
        {
            var claims = new[]
            {
                new Claim("CustomerId", appUser.CustomerId.ToString()),
                // Token içine CustomerId gömüyoruz
                // CreateOrder'da buradan okuyacağız

                new Claim(ClaimTypes.Email, appUser.Email),
                // Kullanıcının emailini de gömüyoruz

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                // Her token için unique bir ID
                // Token'ları birbirinden ayırt etmek için
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            // appsettings.json'dan gizli anahtarı okuyoruz
            // Bu anahtar token'ı imzalamak için kullanılacak
            // "!" → null olmayacağını garanti ediyoruz

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // Token'ı HMAC-SHA256 algoritması ile imzalıyoruz
            // Bu sayede token değiştirilemez

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                // Token'ı kim oluşturdu (bizim uygulama)

                audience: _configuration["Jwt:Audience"],
                // Token kimin için geçerli (bizim API)

                claims: claims,
                // Token içindeki veriler

                expires: DateTime.UtcNow.AddHours(24),
                // Token 24 saat geçerli

                signingCredentials: credentials
            // İmza bilgisi
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            // Token'ı string'e çevirip döndürüyoruz
        }
    }
}
