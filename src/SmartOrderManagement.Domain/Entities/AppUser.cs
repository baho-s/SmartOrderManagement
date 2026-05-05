using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Domain.Entities
{
    public class AppUser:BaseEntity
    {
        public int AppUserId { get; private set; }
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public int CustomerId { get; private set; }
        // Customer ile ilişki AppUser tarafından kuruluyor
        // Customer AppUser'ı bilmiyor 
        public virtual Customer Customer { get; set; } = null!;
        // Navigation property AppUser tarafında kalıyor 
        // Customer → AppUser'ı bilmiyor 
        // AppUser → Customer'ı biliyor 


        //EF Core için parametresiz constructor
        protected AppUser() { }

        public AppUser(string email, string passwordHash, int customerId)
        {
            Email = email;
            PasswordHash = passwordHash;
            CustomerId = customerId;
        }
    }
}
