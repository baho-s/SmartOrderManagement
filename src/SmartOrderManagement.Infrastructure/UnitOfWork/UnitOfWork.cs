using SmartOrderManagement.Application.Interfaces.UnitOfWork;
using SmartOrderManagement.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
