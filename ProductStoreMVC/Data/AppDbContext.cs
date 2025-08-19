using Microsoft.EntityFrameworkCore;
using ProductStoreMVC.Models;

namespace ProductStoreMVC.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        // Seed dữ liệu mẫu
        mb.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Điện thoại" },
            new Category { Id = 2, Name = "Laptop" },
            new Category { Id = 3, Name = "Phụ kiện" }
        );

        mb.Entity<Product>().HasData(
            new Product { Id = 1, Name = "iPhone 14", Price = 18990000, CategoryId = 1 },
            new Product { Id = 2, Name = "ThinkPad X1", Price = 32990000, CategoryId = 2 },
            new Product { Id = 3, Name = "Tai nghe Bluetooth", Price = 499000, CategoryId = 3 }
        );
    }
} 


