using SmartOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartOrderManagement.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);

        IQueryable<Order> GetOrdersAsQueryable();
        Task<List<Order>> GetOrdersListAsync(int page, int pageSize);//IEnumerable nedir? 
        //IEnumerable, bir koleksiyonun üzerinde sırayla dolaşmak için kullanılan bir arayüzdür. Bu arayüz, koleksiyonun elemanlarına erişim
        //sağlar ancak koleksiyonun kendisini değiştirme yeteneği sunmaz. IEnumerable, genellikle LINQ sorguları ve foreach döngüleri
        //gibi durumlarda kullanılır. IEnumerable, koleksiyonun elemanlarını tek tek işlemek için kullanılırken, List gibi
        //koleksiyonlar ise elemanları ekleme, silme gibi işlemler yapabilir.,

        //Kısaca, IEnumerable, koleksiyonun elemanlarına erişim sağlamak için kullanılırken, List gibi koleksiyonlar ise elemanları ekleme, silme gibi işlemler yapabilir.
    }
}
