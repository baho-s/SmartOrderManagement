using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync();

    }
}
