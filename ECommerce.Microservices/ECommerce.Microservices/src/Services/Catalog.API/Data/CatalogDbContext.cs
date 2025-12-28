using Microsoft.EntityFrameworkCore;
using Catalog.API.Models;

namespace Catalog.API.Data;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop", Description = "Gaming laptop 15 inch", Price = 1299.99m, StockQuantity = 10 },
            new Product { Id = 2, Name = "Miš", Description = "Bežični miš", Price = 29.99m, StockQuantity = 50 },
            new Product { Id = 3, Name = "Tastatura", Description = "Mehanička tastatura", Price = 89.99m, StockQuantity = 25 }
        );
    }
}
