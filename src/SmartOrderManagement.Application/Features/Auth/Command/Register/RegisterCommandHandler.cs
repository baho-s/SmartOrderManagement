using MediatR;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Application.Interfaces.Services;
using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Features.Auth.Command.Register
{
    public class RegisterCommandHandler 
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterCommandHandler(IAppUserRepository appUserRepository, ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IJwtService jwtService, IPasswordHasher passwordHasher)
        {
            _appUserRepository = appUserRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> Handle(RegisterCommand command)
        {
            // 1. Email zaten kayıtlı mı kontrol et
            var existingUser = await _appUserRepository.GetByEmailAsync(command.Email);
            if (existingUser != null)
                throw new Exception("Bu email zaten kayıtlı.");

            // 2. Customer oluştur
            var customer = new Customer(
                command.FullName,
                command.Phone,
                command.Email,
                command.Address);
            await _customerRepository.AddAsync(customer);
            await _unitOfWork.CommitAsync();

            // 3. Şifreyi hash'le
            var passwordHash = _passwordHasher.HashPassword(command.Password);
            // Artık BCrypt'i doğrudan kullanmıyoruz
            // IPasswordHasher arkasına gizledik

            // 4. AppUser oluştur
            var appUser = new AppUser(command.Email, passwordHash, customer.CustomerId);
            await _appUserRepository.AddAsync(appUser);
            await _unitOfWork.CommitAsync();

            // 5. Token oluştur ve döndür
            return _jwtService.GenerateToken(appUser);
        }
    }
}
