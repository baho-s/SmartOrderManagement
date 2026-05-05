using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Auth.Command.Login
{
    public class LoginCommandHandler
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public LoginCommandHandler(IAppUserRepository appUserRepository, IJwtService jwtService, IPasswordHasher passwordHasher)
        {
            _appUserRepository = appUserRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Handle(LoginCommand command)
        {
            // 1. Email ile kullanıcıyı bul
            var appUser = await _appUserRepository.GetByEmailAsync(command.Email);
            if (appUser == null)
                throw new Exception("Email veya şifre hatalı.");

            // 2. Şifreyi doğrula
            var isPasswordValid = _passwordHasher.VerifyPassword(command.Password, appUser.PasswordHash);
            if (!isPasswordValid)
                throw new Exception("Email veya şifre hatalı.");

            // 3. Token oluştur ve döndür
            return _jwtService.GenerateToken(appUser);
        }
    }
}
