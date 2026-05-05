using Microsoft.EntityFrameworkCore;
using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Infrastructure.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global Query Filter - Soft Delete
            modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Customer>().HasQueryFilter(cus => !cus.IsDeleted);
            modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<OrderItem>().HasQueryFilter(oi => !oi.IsDeleted);

            // YENİ EKLENEN
            modelBuilder.Entity<AppUser>().HasQueryFilter(u => !u.IsDeleted);
            // Soft delete filter AppUser için de aktif

            // AppUser - Customer 1-1 İlişkisi
            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.Customer)
                // AppUser'ın bir Customer'ı var
                .WithOne()
                // Customer tarafında navigation property YOK
                // .WithOne(c => c.AppUser) yerine boş bıraktık 
                .HasForeignKey<AppUser>(u => u.CustomerId);
            // Foreign Key AppUser tarafında
        }


        //EF Core’un kendi SaveChangesAsync metodunu ezmiş (override etmiş) oluyoruz.
        //Yani artık her: await _context.SaveChangesAync(); çalıştığında 
        //önce bizim kodumuz, sonra EF Core çalışacak.
        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries<BaseEntity>();
            //EF Core’un ChangeTracker mekanizmasına gidiyor.
            //O anda DbContext içinde takip edilen entity’leri alıyor.
            //Sadece BaseEntity’den türeyenleri seçiyor.

            foreach (var entry in entries)
            {
                //Bulunan tüm entity’lerin üzerinden tek tek geçiyoruz.
                //Örnek:
                //
                //   entry → Category
                //  entry → Product
                //   entry → Order
                //
                //   Her kayıt kontrol edilecek.

                if (entry.State == EntityState.Added)//“Bu entity veritabanına YENİ eklenecek.”
                {
                    entry.Entity.CreatedDate=DateTime.UtcNow;//Yeni eklenen kayıt için: CreatedDate= şu anki UTC zamanı.
                }

                if (entry.State == EntityState.Modified)//Bu entity güncellendi.
                {
                    //CreatedDate alanının değişmesini engelle.
                    entry.Property(x => x.CreatedDate).IsModified = false;
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);

            //EK NOT:: SaveChanges override = EF Core pipeline’a middleware(ara katman yazılımı)
            //eklemek gibi. //İstemciden(client) gelen istek (request) ile sunucunun verdiği
            //yanıt (response) arasındaki sürece dahil olan, talepleri işleyen, kontrol eden
            //veya dönüştüren yazılım katmanıdır.
        }
    }
}
