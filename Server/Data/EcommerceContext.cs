using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Server.Models;
using Server.Models.ProductTypes;
using Server.Models.ProductComponents;
namespace Server.Data
{
    public class EcommerceContext : DbContext
    {
        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Image> Images { get; set; }
        // Product Types
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Laptop> Laptops { get; set; }
        public DbSet<Headphones> Headphones { get; set; }
        // Product Components
        public DbSet<Display> Displays { get; set; }
        public DbSet<Cpu> Cpus { get; set; }
        public DbSet<Gpu> Gpus { get; set; }
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<ChargingAccessory> ChargingAccessory { get; set; }
        public DbSet<Ram> Rams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=ecom;Integrated Security=True;TrustServerCertificate=True");
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cpu>()
                .Property(e => e.Brand)
                .HasConversion<string>();

            modelBuilder.Entity<Headphones>()
                .Property(e => e.HeadphoneType)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Products)
                .WithMany(e => e.Orders)
                .UsingEntity<OrderProduct>();
        }
    }
}
