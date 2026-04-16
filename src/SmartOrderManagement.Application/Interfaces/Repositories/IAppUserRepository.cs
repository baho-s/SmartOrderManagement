
using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Repositories
{
    public interface IAppUserRepository
    {
        Task AddAsync(AppUser appUser);
        Task<AppUser?> GetByEmailAsync(string email);
    }
}
