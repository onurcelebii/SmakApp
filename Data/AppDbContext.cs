using Microsoft.EntityFrameworkCore;
using SmakApp.Models;

namespace SmakApp.Data
{
    public class AppDbContext : DbContext
    {
        // DbContextOptions kullanarak veritabanı bağlantısı
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Veritabanında tabloya karşılık gelen DbSet'ler
        public DbSet<User> Users { get; set; } // Kullanıcılar tablosu
        public DbSet<Company> Companies { get; set; } // Şirketler tablosu
        public DbSet<Invitation> Invitations { get; set; } // Davetler tablosu
        public DbSet<Product> Products { get; set; } // Ürünler tablosu
        public DbSet<CompanyProduct> CompanyProducts { get; set; } // Şirket ve ürün arasındaki ilişkiyi temsil eden tablo
        public DbSet<Invite> Invites { get; set; } // E-posta davetler tablosu 

        // İlişkileri ve tablo yapılandırmalarını burada tanımlıyoruz
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Üst sınıfın OnModelCreating metodunu çağırıyoruz
            base.OnModelCreating(modelBuilder);

            // Company (Şirket) tablosunda Bakiye alanı için özel bir veri tipi tanımlıyoruz
            modelBuilder.Entity<Company>()
                .Property(c => c.Balance)
                .HasColumnType("decimal(18,2)"); 

            // CompanyProduct ilişkisi için anahtar ve yapılandırmalar
            modelBuilder.Entity<CompanyProduct>()
                .HasKey(cp => new { cp.CompanyId, cp.ProductId }); // Primary Key olarak CompanyId ve ProductId kullanılıyor

            modelBuilder.Entity<CompanyProduct>()
                .HasOne(cp => cp.Company) // Her CompanyProduct bir Company'ye ait
                .WithMany(c => c.CompanyProducts) // Bir Company'nin birden fazla CompanyProduct kaydı olabilir
                .HasForeignKey(cp => cp.CompanyId); // İlişkiyi CompanyId üzerinden kuruyoruz

            modelBuilder.Entity<CompanyProduct>()
                .HasOne(cp => cp.Product) // Her CompanyProduct bir Product'a ait
                .WithMany(p => p.CompanyProducts) // Bir Product'ın birden fazla CompanyProduct kaydı olabilir
                .HasForeignKey(cp => cp.ProductId); // İlişkiyi ProductId üzerinden kuruyoruz
        }
    }
}
