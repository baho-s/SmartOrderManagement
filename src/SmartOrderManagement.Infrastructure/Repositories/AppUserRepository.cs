using Microsoft.EntityFrameworkCore;
using SmartOrderManagement.Application.Interfaces.Repositories;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Infrastructure.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly AppDbContext _context;

        public AppUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AppUser appUser)
        {
            await _context.AppUsers.AddAsync(appUser);            
        }

        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            return await _context.AppUsers
        .FirstOrDefaultAsync(u => u.Email == email);
            // Include(u => u.Customer) kaldırıldı 
            // Customer navigation property artık AppUser'da yok
        }
    }
}
