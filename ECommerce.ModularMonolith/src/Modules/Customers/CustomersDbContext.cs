using Microsoft.EntityFrameworkCore;
using ECommerce.Modules.Customers.Models;

namespace ECommerce.Modules.Customers;

public class CustomersDbContext : DbContext
{
    public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, FirstName = "Marko", LastName = "Marković", Email = "marko@email.com" },
            new Customer { Id = 2, FirstName = "Jana", LastName = "Janković", Email = "jana@email.com" }
        );
    }
}
