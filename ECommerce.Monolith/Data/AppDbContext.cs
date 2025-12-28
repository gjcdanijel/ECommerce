using Microsoft.EntityFrameworkCore;
using ECommerce.Monolith.Models;

namespace ECommerce.Monolith.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Product)
            .WithMany()
            .HasForeignKey(o => o.ProductId);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Order)
            .WithMany()
            .HasForeignKey(p => p.OrderId);

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop", Description = "Gaming laptop 15 inch", Price = 1299.99m, StockQuantity = 10 },
            new Product { Id = 2, Name = "Miš", Description = "Bežični miš", Price = 29.99m, StockQuantity = 50 },
            new Product { Id = 3, Name = "Tastatura", Description = "Mehanička tastatura", Price = 89.99m, StockQuantity = 25 }
        );

        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, FirstName = "Marko", LastName = "Marković", Email = "marko@email.com" },
            new Customer { Id = 2, FirstName = "Jana", LastName = "Janković", Email = "jana@email.com" }
        );
    }
}